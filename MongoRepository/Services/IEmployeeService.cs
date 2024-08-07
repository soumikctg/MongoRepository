using System.Linq.Expressions;
using MongoDB.Bson;
using MongoRepository.Models;

namespace MongoRepository.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(string id);
        Task<List<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> condition);
        Task AddEmployeeAsync(Employee employee);
        Task AddManyEmployeesAsync(List<Employee> employees);
        Task UpdateEmployeeByIdAsync(string id, Employee employee);
        Task DeleteEmployeeByIdAsync(string id);
        Task DeleteAllEmployeesAsync();
        Task<List<Dictionary<string, object>>> GetFieldsOfEmployeesByAsync(List<string> fields);
        Task<List<EmployeeDto>> GetProjectedEmployeesAsync();

    }
}
