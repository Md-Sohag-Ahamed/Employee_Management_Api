using EmployeeManagement_WepApi.Models;
using EmployeeManagement_WepApi.Repository;
using EmployeeManagement_WepApi.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using OtpNet;
using System.Net.Mail;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Crypto.Generators;
using OtpNet;

namespace EmployeeManagement_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOTPRepository _otpRepository;

        public UserController(IUserRepository userRepository, IOTPRepository otpRepository)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegistrationRequestModel registrationRequest)
        {
            if (registrationRequest == null || string.IsNullOrEmpty(registrationRequest.Username) || string.IsNullOrEmpty(registrationRequest.Password) || string.IsNullOrEmpty(registrationRequest.Email))
            {
                return BadRequest("Invalid registration request.");
            }
            if (_userRepository.IsUsernameTaken(registrationRequest.Username) || _userRepository.IsEmailTaken(registrationRequest.Email))
            {
                return BadRequest("Username or email is already taken.");
            }
            string hashedPassword = HashPassword(registrationRequest.Password);

            var user = new UserModel
            {
                Username = registrationRequest.Username,
                PasswordHash = hashedPassword,
                Email = registrationRequest.Email,
                IsVerified=true 
            };
            _userRepository.AddUser(user);
            return Ok("User registered successfully");
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = _userRepository.GetUserByUsername(loginRequest.Username);

            if (user == null || user.PasswordHash != loginRequest.Password)
            {
                return BadRequest("Invalid username or password");
            }

            if (!user.IsVerified)
            {
                return BadRequest("User is not verified. Please complete OTP verification.");
            }
            var otp = GenerateOTP(user.Id);
            SendOTPToUserEmail(user.Email, otp);
            var otpModel = new OTPModel
            {
                UserId = user.Id,
                OTP = otp
            };
            _otpRepository.AddOTP(otpModel);
            SaveOTPToDatabase(user.Id, otp);
            return Ok("Login successful. OTP sent to user's email for verification.");
        }
        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP([FromBody] OTPVerificationRequestModel otpVerificationRequest)
        {
            if (otpVerificationRequest == null || string.IsNullOrEmpty(otpVerificationRequest.username) || string.IsNullOrEmpty(otpVerificationRequest.OTP))
            {
                return BadRequest("Username and OTP are required for verification.");
            }
            var user = _userRepository.GetUserByUsername(otpVerificationRequest.username);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var latestOTP = _otpRepository.GetOTPByUserId(user.Id);
            if (latestOTP == null || latestOTP.OTP != otpVerificationRequest.OTP)
            {
                return BadRequest("Invalid OTP.");
            }
            user.IsVerified = true;
            _userRepository.UpdateUser(user);

            return Ok("OTP verification successful. User is now verified.");
        }

        private string GenerateOTP(int userId)
        {
            string secretKey = _userRepository.GetSecretKeyForUser(userId);
            if (string.IsNullOrEmpty(secretKey))
            {
                Console.WriteLine("Invalid Secret Key: Secret key is null or empty.");
                return "Invalid Secret Key";
            }

            Console.WriteLine($"Original Secret Key: {secretKey}");
            secretKey = secretKey.TrimEnd('=');
            Console.WriteLine($"Trimmed Secret Key: {secretKey}");

            try
            {
                
                var keyBytes =Convert.FromBase64String(secretKey);
                Console.WriteLine($"Key Bytes: {string.Join(", ", keyBytes)}");
                var totp = new Totp(keyBytes, step: 30);
                var otp = totp.ComputeTotp(DateTime.UtcNow.AddMinutes(1));
                return otp;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error in GenerateOTP: {ex.Message}");
                return "Error Generating OTP";
            }

        }
        private void SendOTPToUserEmail(string userEmail, string otp)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("MyOTPAPP", "sohagahamed81995@gmail.com"));
                message.To.Add(new MailboxAddress("otp", userEmail));
                message.Subject = "OTP Verification";

                var textPart = new TextPart("plain")
                {
                    Text = $"Your OTP for verification is: {otp}"
                };

                var multipart = new MimeKit.Multipart("mixed");
                multipart.Add(textPart);
                message.Body = multipart;

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.ethereal.email", 587, false);
                    client.Authenticate("antwon.ledner28@ethereal.email", "7TFhM58QPbYbU18fxX");
                    client.Send(message);
                    client.Disconnect(true);
                    Console.WriteLine($"Email sent successfully to {userEmail} with OTP: {otp}");
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        private void SaveOTPToDatabase(int userId, string otp)
        {
            var otpModel = new OTPModel
            {
                UserId = userId,
                OTP = otp
            };
            _otpRepository.AddOTP(otpModel);
        }
    }
}
