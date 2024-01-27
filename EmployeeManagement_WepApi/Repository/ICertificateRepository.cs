using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface ICertificateRepository
    {
        //IEnumerable<CertificateModel> GetCertificates();
        //CertificateModel GetCertificateById(int certificateId);
        //void AddCertificate(CertificateModel certificate);
        void AddCertificate(CertificateModel certificate);
        List<CertificateModel> GetCertificatesByUserId(int userId);
        CertificateModel GetCertificateById(int certificateId);
        void UpdateCertificate(CertificateModel certificate);

    }
}
