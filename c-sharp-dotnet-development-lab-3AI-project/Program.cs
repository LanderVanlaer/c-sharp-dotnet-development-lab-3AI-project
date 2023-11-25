using System.Configuration;
using System.Text;
using c_sharp_dotnet_development_lab_3AI_project.database;
using c_sharp_dotnet_development_lab_3AI_project.filters;
using c_sharp_dotnet_development_lab_3AI_project.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => { options.Filters.Add<ApiValidationFilterAttribute>(); })
    //https://stackoverflow.com/a/59210264/13165967
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IRepository, DatabaseRepository>();

#pragma warning disable CS0618 // Type or member is obsolete
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"] ??
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

//https://mirsaeedi.medium.com/asp-net-core-customize-validation-error-message-9022c12d3d7d
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

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