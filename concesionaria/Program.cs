
using Concesionario.Application.Interfaces;
using Concesionario.Application.Services;
using Concesionario.Application.Services.Core;
using Concesionario.Application.Services.Vehicles;
using Concesionario.Data;
using Concesionario.Data.Identity;
using Concesionario.Data.Repositories;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

namespace Concesionario
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ConcesionarioDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Concesionario"));
            });



            builder.Services.AddIdentity<ApplicationUserIdentity, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
.AddEntityFrameworkStores<ConcesionarioDbContext>()
.AddDefaultTokenProviders();


            //Inyeccion de servicios

            builder.Services.AddScoped<IRepository, EfRepository>();
            builder.Services.AddScoped<IVehicleService,VehicleManagementService>();
            builder.Services.AddScoped<IUserService,UserManagementService>();

            ///

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAuthorization();
            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

            builder.Services.AddScoped<JwtTokenService>();

            builder.Services.AddCors(options => {
                options.AddPolicy("AllowFronted", policy =>
                {

                    policy.WithOrigins("http://localhost:7168")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
                
            });
            builder.Services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName); 
            });

            builder.Services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "AuthTest",
                    Version = "v1",
                    Description = "Autenticacion con Identity"
                });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Ingrese el JWT token con formato: Bearer {token}",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
    {
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Reference = new Microsoft.OpenApi.Models.OpenApiReference
            {
                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] {}
    }
});
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
