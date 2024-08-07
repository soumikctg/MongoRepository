using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository.Models;
using MongoRepository.Repository;

namespace MongoRepository.Services
{
    public class EmployeeService(IDatabaseContext context) : IEmployeeService
    {
        public string ClientDatabase { get; set; } = "_soumik-client";
        public string GlobalDatabase { get; set; } = "_soumik-global";
        public async Task AddEmployeeAsync(Employee employee)
        {
            await context.SaveAsync(GlobalDatabase, employee);
        }

        public async Task AddManyEmployeesAsync(List<Employee> employees)
        {
            await context.SaveManyAsync(GlobalDatabase, employees);
        }

        public async Task DeleteAllEmployeesAsync()
        {
            await context.DeleteItemsAsync<Employee>(GlobalDatabase);
        }

        public async Task DeleteEmployeeByIdAsync(string id)
        {
            await context.DeleteItemByIdAsync<Employee>(GlobalDatabase, id);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await context.GetItemsAsync<Employee>(GlobalDatabase);
        }

        public async Task<Employee> GetEmployeeByIdAsync(string id)
        {
            return await context.GetItemByIdAsync<Employee>(GlobalDatabase, id);
        }

        public async Task<List<Employee>> GetEmployeesByConditionAsync(Expression<Func<Employee, bool>> condition)
        {
            return await context.GetItemsByConditionAsync(GlobalDatabase, condition);
        }

        public async Task UpdateEmployeeByIdAsync(string id, Employee employee)
        {
            await context.UpdateItemByIdAsync(GlobalDatabase, id, employee);
        }

        public async Task<List<Dictionary<string, object>>> GetFieldsOfEmployeesByAsync(List<string> fields)
        {
            return await context.GetFieldsOfItemsByAsync<Employee>(GlobalDatabase, fields);
        }

        public async Task<List<EmployeeDto>> GetProjectedEmployeesAsync()
        {
            var projectionDef = Builders<Employee>.Projection.Expression(e => new EmployeeDto
            {
                FullName = e.FullName,
                PhoneNumber = e.PhoneNumber
            });

            return await context.GetProjectedItemsAsync(GlobalDatabase, projectionDef);
        }
    }
}
