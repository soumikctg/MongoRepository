using MongoRepository.Models;

namespace MongoRepository.Services
{
    public interface IDatabaseTransactionService
    {
        Task AddEmployeeAndDepartmentAsync(Employee employee, Department department);
    }
}
