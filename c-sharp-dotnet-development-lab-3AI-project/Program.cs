using System.Configuration;
using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); //add controller classes to be used
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IRepository, DatabaseRepository>();

string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"] ??
#pragma warning disable CS0618 // Type or member is obsolete
                          throw new ConfigurationException(
                              "Could not get \"ConnectionStrings:DefaultConnection\" from appsettings.json"
                          );
#pragma warning restore CS0618 // Type or member is obsolete

builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); //redirect to https

app.UseAuthorization(); //allow [authorize] attribute in controller
app.MapControllers(); //map all controller classes
app.Run();