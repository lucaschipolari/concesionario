
using Concesionario.Application.Configuration;
using Concesionario.Application.Interfaces;
using Concesionario.Application.Services;
using Concesionario.Application.Services.Branch;
using Concesionario.Application.Services.Core;
using Concesionario.Application.Services.SalesReservation;
using Concesionario.Application.Services.Vehicles;
using Concesionario.Data;
using Concesionario.Data.Identity;
using Concesionario.Data.Repositories;
using Concesionario.Data.Services;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.SalesReservation;
using Concesionario.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
            builder.Services.Configure<TaxSettings>(
            builder.Configuration.GetSection("TaxSettings"));

            builder.Services.AddScoped<IRepository, EfRepository>();
            builder.Services.AddScoped<IVehicleService,VehicleManagementService>();
            builder.Services.AddScoped<IUserService,UserManagementService>();
            builder.Services.AddScoped<IBranchService, BranchManagementService>();
            builder.Services.AddScoped<IPositionService,PositionManagementService>();
            builder.Services.AddScoped<ISaleService,SaleManagementService>();
            builder.Services.AddScoped<ICurrentUserService, UserCurrentService>();

            builder.Services.AddScoped<TaxCalculator>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<TaxSettings>>().Value;
                return new TaxCalculator(settings.IVA_RATE, settings.STAMP_TAX_RATE);
            });
            ///
        

            builder.Services.AddHttpContextAccessor();
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
            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var keyText = jwtConfig["Key"] ?? throw new ArgumentNullException("JWT Key");
            var key = Encoding.UTF8.GetBytes(keyText);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
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
