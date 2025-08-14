using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Concesionario.Domain.Entities.Core;
using Concesionario.Data.Identity;

namespace Concesionario.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserIdentity>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Renombrar tablas de Identity
            builder.Entity<ApplicationUserIdentity>().ToTable("UsersIdentity");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UsersClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UsersLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RolesClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UsersTokens");

            // Configuración de entidades personalizadas
            builder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
            });

            builder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employees");
                entity.HasOne(e => e.User)
                      .WithOne(u => u.Employee)
                      .HasForeignKey<Employee>(e => e.UserId)
                      .IsRequired();
            });

            builder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers");
                entity.HasOne(c => c.User)
                      .WithOne(u => u.Customer)
                      .HasForeignKey<Customer>(c => c.UserId)
                      .IsRequired();
            });
        }
    }
}