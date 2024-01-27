namespace EmployeeManagement_WepApi.Models
{
    public class CertificateModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeModel Employee { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}
