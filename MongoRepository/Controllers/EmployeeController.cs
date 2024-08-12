using Microsoft.AspNetCore.Mvc;
using MongoRepository.Models;
using MongoRepository.Services;

namespace MongoRepository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController(IEmployeeService service, IDatabaseTransactionService transactionService) : ControllerBase
    {
        private readonly IEmployeeService EmployeeService = service;
        private readonly IDatabaseTransactionService TransactionService = transactionService;

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(Employee employee)
        {
            try
            {
                await EmployeeService.AddEmployeeAsync(employee);
                return CreatedAtAction(nameof(AddEmployee), employee);
            }
            catch (Exception e)
            {
                return BadRequest(new { sucess = false, message = e.Message });
            }

        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await EmployeeService.GetAllEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

        }

        [HttpGet("GetProjectedEmployees")]
        public async Task<IActionResult> GetProjectedEmployees()
        {
            try
            {
                var employees = await EmployeeService.GetProjectedEmployeesAsync();
                return Ok(employees);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

        }

        [HttpPost("AddEmployeeAndDepartment")]
        public async Task<IActionResult> AddEmployeeAndDept(EmployeeDepartmentDto data)
        {
            try
            {
                await TransactionService.AddEmployeeAndDepartmentAsync(data.employee, data.department);
                return CreatedAtAction(nameof(AddEmployeeAndDept), data);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }
    }
}
