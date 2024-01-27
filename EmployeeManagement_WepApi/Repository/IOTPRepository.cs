using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface IOTPRepository
    {
        OTPModel GetOTPByUserId(int userId);
        void AddOTP(OTPModel otp);
        void RemoveOTP(int userId);
        UserModel GetUserById(int userId);
        void UpdateUser(UserModel user);
    }
}
