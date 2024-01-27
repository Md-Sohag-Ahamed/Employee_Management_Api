using EmployeeManagement_WepApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace EmployeeManagement_WepApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<int, string> _userSecretKeys = new Dictionary<int, string>();

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _userSecretKeys = new Dictionary<int, string>();
        }
        public UserModel GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }
        public UserModel GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public void AddUser(UserModel user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void UpdateUser(UserModel user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public string GetSecretKeyForUser(int userId)
        {
            byte[] secretKeyBytes = new byte[6];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(secretKeyBytes);
            }
            return Convert.ToBase64String(secretKeyBytes);

        }
        public bool IsUsernameTaken(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }
        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int GetUserRole(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user?.Role ?? 0;
        }
    }
}
