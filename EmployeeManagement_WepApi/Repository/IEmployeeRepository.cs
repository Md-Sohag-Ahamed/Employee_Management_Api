using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface IEmployeeRepository
    {
        IEnumerable<EmployeeModel> GetEmployees();
        EmployeeModel GetEmployeeById(int employeeId);
        void AddEmployee(EmployeeModel employee);
    }
}
