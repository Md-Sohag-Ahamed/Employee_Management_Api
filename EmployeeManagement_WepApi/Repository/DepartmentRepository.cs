using EmployeeManagement_WepApi.Models;

namespace EmployeeManagement_WepApi.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DepartmentModel> GetDepartments()
        {
            return _context.Departments.ToList();
        }

        public DepartmentModel GetDepartmentById(int departmentId)
        {
            return _context.Departments.SingleOrDefault(d => d.Id == departmentId);
        }

        public void AddDepartment(DepartmentModel department)
        {
            _context.Departments.Add(department);
            _context.SaveChanges();
        }
    }
}
