using MongoRepository.Models;
using System.Linq.Expressions;

namespace MongoRepository.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(string id);
        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentByIdAsync(string id, Department department);
        Task DeleteDepartmentByIdAsync(string id);
        Task DeleteAllDepartmentsAsync();
        Task<List<DepartmentDto>> GetProjectedDepartmentsAsync();
    }
}
