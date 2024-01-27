using EmployeeManagement_WepApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement_WepApi.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<EmployeeModel> GetEmployees()
        {
            return _context.Employees.Include(e => e.Department).ToList();
        }

        public EmployeeModel GetEmployeeById(int employeeId)
        {
            return _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == employeeId);
        }

        public void AddEmployee(EmployeeModel employee)
        {
            _context.Employees.Add(employee);

            // Create a default certificate for the new employee
            var certificate = new CertificateModel
            {
                EmployeeId = employee.Id,
                Date = DateTime.Now
                                    
            };
            _context.Certificates.Add(certificate);

            _context.SaveChanges();
        }
    }
}
