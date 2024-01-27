namespace EmployeeManagement_WepApi.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public List<EmployeeModel> Employees { get; set; }
        public OTPModel OTP { get; set; }
        public int Role { get; set; }
        public ICollection<CertificateModel> Certificates { get; set; }
    }
}
