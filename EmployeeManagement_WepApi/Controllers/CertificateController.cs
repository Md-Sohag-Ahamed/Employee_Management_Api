using EmployeeManagement_WepApi.Models;
using EmployeeManagement_WepApi.Repository;
using EmployeeManagement_WepApi.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Security.Claims;

namespace EmployeeManagement_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPdfService _pdfService;
        private readonly IUserRepository _userRepository;

        public CertificateController(
            ICertificateRepository certificateRepository,
            IEmployeeRepository employeeRepository,
            IPdfService pdfService,
            IUserRepository userRepository
            )
        {
            _certificateRepository = certificateRepository;
            _employeeRepository = employeeRepository;
            _pdfService = pdfService;
            _userRepository = userRepository;
        }

        [HttpPost("create")]
        public IActionResult CreateCertificate([FromBody] CertificateRequestModel certificateRequest)
        {
            int userId = GetUserIdFromClaims();

            var certificate = new CertificateModel
            {
                EmployeeId = certificateRequest.EmployeeId,
                Date = DateTime.Now,
                UserId = userId
            };

            _certificateRepository.AddCertificate(certificate);

            return Ok("Certificate created successfully");
        }

        [HttpGet("list")]
        public IActionResult ListCertificates()
        {
            int userId = GetUserIdFromClaims();

            var certificates = _certificateRepository.GetCertificatesByUserId(userId);
            return Ok(certificates);
        }

        [HttpGet("details/{certificateId}")]
        public IActionResult GetCertificateDetails(int certificateId)
        {
            int userId = GetUserIdFromClaims();

            var certificate = _certificateRepository.GetCertificateById(certificateId);

            if (certificate == null)
            {
                return NotFound("Certificate not found");
            }

            if (certificate.UserId != userId)
            {
                return Unauthorized("You don't have permission to access this certificate");
            }

            return Ok(certificate);
        }

        [HttpPut("edit/{certificateId}")]
        public IActionResult EditCertificate(int certificateId, [FromBody] CertificateRequestModel updatedCertificate)
        {
            int userId = GetUserIdFromClaims();

            var certificate = _certificateRepository.GetCertificateById(certificateId);

            if (certificate == null)
            {
                return NotFound("Certificate not found");
            }

            if (certificate.UserId != userId)
            {
                return Unauthorized("You don't have permission to edit this certificate");
            }
            certificate.EmployeeId = updatedCertificate.EmployeeId;
            _certificateRepository.UpdateCertificate(certificate);
            return Ok("Certificate updated successfully");
        }

        [HttpGet("download/{certificateId}")]
        public IActionResult DownloadCertificate(int certificateId)
        {
            var certificate = _certificateRepository.GetCertificateById(certificateId);

            if (certificate == null)
            {
                return NotFound("Certificate not found");
            }

            int userId = GetUserIdFromClaims();
            if (!IsUserAdmin(userId))
            {
                return Unauthorized("Only admins can download certificates");
            }
            var employee = _employeeRepository.GetEmployeeById(certificate.EmployeeId);
            var pdfBytes = _pdfService.GenerateCertificatePdf(employee, certificate);
            return File(pdfBytes, "application/pdf", $"Certificate_{certificateId}.pdf");
        }
        private bool IsUserAdmin(int userId)
        {
            string userRole = _userRepository.GetUserRole(userId).ToString();
            return string.Equals(userRole, "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private int GetUserIdFromClaims()
        {
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return -1;
        }
    }
}
