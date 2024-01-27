using EmployeeManagement_WepApi.Models;
using EmployeeManagement_WepApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement_WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IActionResult GetEmployees()
        {
            var employees = _employeeRepository.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("{employeeId}")]
        public IActionResult GetEmployeeById(int employeeId)
        {
            var employee = _employeeRepository.GetEmployeeById(employeeId);

            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(employee);
        }

        [HttpPost("create")]
        public IActionResult CreateEmployee([FromBody] EmployeeModel employee)
        {
            if (employee == null)
            {
                return BadRequest("Invalid employee data");
            }

            _employeeRepository.AddEmployee(employee);

            return Ok("Employee created successfully");
        }
    }
}
