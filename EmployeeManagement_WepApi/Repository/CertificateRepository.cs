using EmployeeManagement_WepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement_WepApi.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        //public IEnumerable<CertificateModel> GetCertificates()
        //{
        //    return _context.Certificates.Include(c => c.Employee).ToList();
        //}
        public CertificateModel GetCertificateById(int certificateId)
        {
            return _context.Certificates.Include(c => c.Employee).SingleOrDefault(c => c.Id == certificateId);
        }
        public void AddCertificate(CertificateModel certificate)
        {
            _context.Certificates.Add(certificate);
            _context.SaveChanges();
        }

        public List<CertificateModel> GetCertificatesByUserId(int userId)
        {
            return _context.Certificates.Where(c => c.UserId == userId).ToList();
        }

        public void UpdateCertificate(CertificateModel certificate)
        {
            var existingCertificate = _context.Certificates.FirstOrDefault(c => c.Id == certificate.Id);
            if (existingCertificate != null)
            {
                existingCertificate.EmployeeId = certificate.EmployeeId;
            }
        }
    }
}
