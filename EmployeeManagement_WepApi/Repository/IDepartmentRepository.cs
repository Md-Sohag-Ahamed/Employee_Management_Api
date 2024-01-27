using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public interface IDepartmentRepository
    {
        IEnumerable<DepartmentModel> GetDepartments();
        DepartmentModel GetDepartmentById(int departmentId);
        void AddDepartment(DepartmentModel department);
    }
}
