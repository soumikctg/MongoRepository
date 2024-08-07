using Microsoft.AspNetCore.Mvc;
using MongoRepository.Models;
using MongoRepository.Services;

namespace MongoRepository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController(IDepartmentService service, IEmployeeService empService) : ControllerBase
    {
        private readonly IDepartmentService DepartmentService = service;
        private readonly IEmployeeService EmployeeService = empService;

        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            try
            {
                await DepartmentService.AddDepartmentAsync(department);
                return CreatedAtAction(nameof(AddDepartment), department);
            }
            catch (Exception e)
            {
                return BadRequest(new { sucess = false, message = e.Message });
            }
        }

        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                var departments = await DepartmentService.GetAllDepartmentsAsync();
                var employees = await EmployeeService.GetAllEmployeesAsync();
                return Ok(departments);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

        [HttpGet("GetProjectedDepartments")]
        public async Task<IActionResult> GetProjectedDepartments()
        {
            try
            {
                var departments = await DepartmentService.GetProjectedDepartmentsAsync();
                return Ok(departments);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }
        }

    }
}
