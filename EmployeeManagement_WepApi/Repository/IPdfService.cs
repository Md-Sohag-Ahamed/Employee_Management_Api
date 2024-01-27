using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface IPdfService
    {
        byte[] GenerateCertificatePdf(EmployeeModel employee, CertificateModel certificate);
    }

}
