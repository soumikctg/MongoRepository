using MongoDB.Driver;
using MongoRepository.DatabaseConfig;
using MongoRepository.Repository;
using MongoRepository.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var mongoDbSettings = configuration.GetSection("MongoDBSettings").Get<DatabaseSettings>();
var client = new MongoClient(mongoDbSettings?.ConnectionString);
builder.Services.AddSingleton<IMongoClient>(client);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped(typeof(IDatabaseContext), typeof(DatabaseContext));
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
