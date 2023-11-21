using System.Configuration;
using System.Text;
using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options =>
        {
            builder.Configuration.Bind("JwtSettings", options);

            options.TokenValidationParameters.IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SigningKey"] ??
                                           throw new InvalidOperationException(
                                               "Config JwtSettings:SigningKey is null"))
                );
        });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); //redirect to https

app.UseAuthentication();
app.UseAuthorization(); //allow [authorize] attribute in controller

app.MapControllers(); //map all controller classes
app.Run();