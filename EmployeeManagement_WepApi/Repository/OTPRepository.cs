using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public class OTPRepository : IOTPRepository
    {
        private readonly ApplicationDbContext _context;

        public OTPRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public OTPModel GetOTPByUserId(int userId)
        {
            return _context.OTPs.SingleOrDefault(otp => otp.UserId == userId);
        }
        public void AddOTP(OTPModel otp)
        {
            _context.OTPs.Add(otp);
            _context.SaveChanges();
        }

        public void RemoveOTP(int userId)
        {
            var existingOTP = _context.OTPs.SingleOrDefault(otp => otp.UserId == userId);

            if (existingOTP != null)
            {
                _context.OTPs.Remove(existingOTP);
                _context.SaveChanges();
            }
        }

        public UserModel GetUserById(int userId)
        {
            return _context.Users.SingleOrDefault(user => user.Id == userId);
        }

        public void UpdateUser(UserModel user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
