using MongoRepository.Models;
using MongoRepository.Repository;

namespace MongoRepository.Services
{
    public class DatabaseTransactionService(IDatabaseContext context):IDatabaseTransactionService
    {
        public string ClientDatabase { get; set; } = "_soumik-client";
        public string GlobalDatabase { get; set; } = "_soumik-global";


        public async Task AddEmployeeAndDepartmentAsync(Employee employee, Department department)
        {
            await context.ExecuteTransactionAsync(async session =>
            {
                await context.SaveAsync(ClientDatabase, employee);

                await context.SaveAsync(ClientDatabase, department);

            });
        }

    }
}
