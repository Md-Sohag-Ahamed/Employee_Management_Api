namespace EmployeeManagement_WepApi.Models
{
    public class OTPModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
        public string OTP { get; set; }
    }
}
