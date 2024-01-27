using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface IUserRepository
    {
        UserModel GetUserByUsername(string username);
        UserModel GetUserByEmail(string email);
        void AddUser(UserModel user);
        void UpdateUser(UserModel user);
        string GetSecretKeyForUser(int userId);
        bool IsUsernameTaken(string username);
        bool IsEmailTaken(string email);
        int GetUserRole(int userId);
    }
}
