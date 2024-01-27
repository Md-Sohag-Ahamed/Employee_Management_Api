namespace EmployeeManagement_WepApi.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentModel Department { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public int CertificateId { get; set; }
        public CertificateModel Certificate { get; set; }
    }
}
