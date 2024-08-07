using MongoDB.Driver;
using MongoRepository.Models;
using MongoRepository.Repository;

namespace MongoRepository.Services
{
    public class DepartmentService(IDatabaseContext context) : IDepartmentService
    {
        public string ClientDatabase { get; set; } = "_soumik-client";
        public string GlobalDatabase { get; set; } = "_soumik-global";

        public async Task AddDepartmentAsync(Department department)
        {
            await context.SaveAsync(ClientDatabase, department);
        }

        public async Task DeleteAllDepartmentsAsync()
        {
            await context.DeleteItemsAsync<Department>(ClientDatabase);
        }

        public async Task DeleteDepartmentByIdAsync(string id)
        {
            await context.DeleteItemByIdAsync<Department>(ClientDatabase, id);
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            return await context.GetItemsAsync<Department>(ClientDatabase);
        }

        public async Task<Department> GetDepartmentByIdAsync(string id)
        {
            return await context.GetItemByIdAsync<Department>(ClientDatabase, id);
        }

        public async Task UpdateDepartmentByIdAsync(string id, Department department)
        {
            await context.UpdateItemByIdAsync(ClientDatabase, id, department);
        }

        public async Task<List<DepartmentDto>> GetProjectedDepartmentsAsync()
        {
            var projectionDef = Builders<Department>.Projection.Expression(d => new DepartmentDto
            {
                DepartmentName = d.DepartmentName,
            });

            return await context.GetProjectedItemsAsync(ClientDatabase, projectionDef);
        }
    }
}
