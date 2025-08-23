using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.Finance;
using Concesionario.Domain.Entities.SalesReservation;
using Concesionario.Domain.Entities.Vehicles;
using Concesionario.Data.Identity;


namespace Concesionario.Data
    {
        public class ConcesionarioDbContext : IdentityDbContext<ApplicationUserIdentity, IdentityRole<Guid>, Guid>
    {
            public ConcesionarioDbContext(DbContextOptions<ConcesionarioDbContext> options)
                : base(options)
            {
            }

            // =========================
            // DbSet de todas las entidades
            // =========================
            public DbSet<User> UsersCustom { get; set; } // para evitar conflicto con Identity
            public DbSet<Employee> Employees { get; set; }
            public DbSet<Customer> Customers { get; set; }
            public DbSet<Position> Positions { get; set; }
            public DbSet<Reservation> Reservations { get; set; }
            public DbSet<Sale> Sales { get; set; }
            public DbSet<FinancingPlan> FinancingPlans { get; set; }
            public DbSet<Installment> Installments { get; set; }
            public DbSet<Payment> Payments { get; set; }
            public DbSet<VehicleBrand> VehicleBrands { get; set; }
            public DbSet<VehicleModel> VehicleModels { get; set; }
            public DbSet<Vehicle> Vehicles { get; set; }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

            // =========================
            // Renombrar tablas Identity
            // =========================
            builder.Entity<ApplicationUserIdentity>().ToTable("UsersIdentity");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UsersRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UsersClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UsersLogins");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RolesClaims");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UsersTokens");

            // =========================
            // Configuración de User
            // =========================
            builder.Entity<User>(entity =>
                {
                    entity.ToTable("Users");
                    entity.Property(u => u.FullName).IsRequired().HasMaxLength(150);
                    entity.HasIndex(u => u.Email).IsUnique();
                    entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
                    entity.HasIndex(u => u.DNI).IsUnique();
                    entity.Property(u => u.Phone).IsRequired().HasMaxLength(20);
                    entity.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
                    entity.Property(u => u.UserType).IsRequired();


                });

                // =========================
                // Configuración de Employee
                // =========================
                builder.Entity<Employee>(eb =>
                {
                    eb.ToTable("Employees");
                    eb.HasKey(e => e.Id);

                    eb.HasOne(e => e.User)
                        .WithOne(u => u.Employee)
                        .HasForeignKey<Employee>(e => e.UserId)
                        .IsRequired();

                    eb.HasOne(e => e.Branch)
                        .WithMany()
                        .HasForeignKey(e => e.BranchId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // Configuración de Customer
                // =========================
                builder.Entity<Customer>(eb =>
                {
                    eb.ToTable("Customers");
                    eb.HasKey(c => c.Id);

                    eb.HasOne(c => c.User)
                        .WithOne(u => u.Customer)
                        .HasForeignKey<Customer>(c => c.UserId)
                        .IsRequired();

                    eb.HasMany(c => c.Sales)
                        .WithOne(s => s.Customer)
                        .HasForeignKey(s => s.CustomerId)
                        .OnDelete(DeleteBehavior.Restrict);

                    eb.HasMany(c => c.Reservations)
                        .WithOne(r => r.Customer)
                        .HasForeignKey(r => r.CustomerId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // Positions
                // =========================
                builder.Entity<Position>(eb =>
                {
                    eb.ToTable("Positions");
                    eb.HasKey(r => r.Id);

                    eb.Property(r => r.Name).HasMaxLength(50).IsRequired();
                    eb.Property(r => r.Descripcion).HasMaxLength(100).IsRequired();
                });

                // =========================
                // Reservations
                // =========================
                builder.Entity<Reservation>(eb =>
                {
                    eb.ToTable("Reservations");
                    eb.HasKey(r => r.Id);

                    eb.Property(r => r.StartDate).IsRequired();
                    eb.Property(r => r.EndDate).IsRequired();
                    eb.Property(r => r.ReservationAmount).HasPrecision(15, 2).IsRequired();
                    eb.Property(r => r.Status).IsRequired();

                    eb.HasOne(r => r.Customer)
                        .WithMany(c => c.Reservations)
                        .HasForeignKey(r => r.CustomerId)
                        .OnDelete(DeleteBehavior.Restrict);

                    eb.HasOne<Vehicle>()
                        .WithMany()
                        .HasForeignKey(r => r.VehicleId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // Sales
                // =========================
                builder.Entity<Sale>(eb =>
                {
                    eb.ToTable("Sales");
                    eb.HasKey(s => s.Id);

                    eb.Property(s => s.SaleDate).IsRequired();
                    eb.Property(s => s.TotalAmount).HasPrecision(15, 2).IsRequired();
                    eb.Property(s => s.Status).IsRequired();

                    eb.HasOne(s => s.Customer)
                        .WithMany(c => c.Sales)
                        .HasForeignKey(s => s.CustomerId)
                        .OnDelete(DeleteBehavior.Restrict);

                    eb.HasOne<Reservation>()
                        .WithMany()
                        .HasForeignKey(s => s.ReservationId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // FinancingPlan
                // =========================
                builder.Entity<FinancingPlan>(eb =>
                {
                    eb.ToTable("FinancingPlans");
                    eb.HasKey(fp => fp.Id);

                    eb.Property(fp => fp.SaleId).IsRequired();
                    eb.Property(fp => fp.TotalAmount).HasPrecision(15, 2).IsRequired();
                    eb.Property(fp => fp.NumberOfInstallments).IsRequired();
                    eb.Property(fp => fp.InterestRate).HasPrecision(5, 2).IsRequired();
                    eb.Property(fp => fp.StartDate).IsRequired();

                    eb.HasMany(fp => fp.Installments)
                        .WithOne()
                        .HasForeignKey(i => i.FinancingPlanId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

                // =========================
                // Installment
                // =========================
                builder.Entity<Installment>(eb =>
                {
                    eb.ToTable("Installments");
                    eb.HasKey(i => i.Id);

                    eb.Property(i => i.FinancingPlanId).IsRequired();
                    eb.Property(i => i.Number).IsRequired();
                    eb.Property(i => i.Amount).HasPrecision(15, 2).IsRequired();
                    eb.Property(i => i.DueDate).IsRequired();
                    eb.Property(i => i.Status).IsRequired();

                    eb.HasMany(i => i.Payments)
                        .WithOne()
                        .HasForeignKey(p => p.InstallmentId)
                        .OnDelete(DeleteBehavior.Cascade);
                });

                // =========================
                // Payment
                // =========================
                builder.Entity<Payment>(eb =>
                {
                    eb.ToTable("Payments");
                    eb.HasKey(p => p.Id);

                    eb.Property(p => p.InstallmentId).IsRequired();
                    eb.Property(p => p.PaymentDate).IsRequired();
                    eb.Property(p => p.Amount).HasPrecision(15, 2).IsRequired();
                    eb.Property(p => p.Method).IsRequired();
                    eb.Property(p => p.Status).IsRequired();
                });

                // =========================
                // VehicleBrand
                // =========================
                builder.Entity<VehicleBrand>(eb =>
                {
                    eb.ToTable("VehicleBrands");
                    eb.HasKey(b => b.Id);

                    eb.HasIndex(b => b.Name).IsUnique();

                    eb.Property(b => b.Name).HasMaxLength(100).IsRequired();
                    eb.Property(b => b.Country).HasMaxLength(100).IsRequired();

                    eb.HasMany(b => b.Models)
                        .WithOne(m => m.Brand)
                        .HasForeignKey(m => m.BrandId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // VehicleModel
                // =========================
                builder.Entity<VehicleModel>(eb =>
                {
                    eb.ToTable("VehicleModels");
                    eb.HasKey(m => m.Id);

                    eb.Property(m => m.Name).HasMaxLength(100).IsRequired();
                    eb.Property(m => m.BrandId).IsRequired();

                    eb.HasOne(m => m.Brand)
                        .WithMany(b => b.Models)
                        .HasForeignKey(m => m.BrandId)
                        .OnDelete(DeleteBehavior.Restrict);

                    eb.HasMany(m => m.Vehicles)
                        .WithOne(v => v.Model)
                        .HasForeignKey(v => v.ModelId)
                        .OnDelete(DeleteBehavior.Restrict);
                });

                // =========================
                // Vehicle
                // =========================
                builder.Entity<Vehicle>(eb =>
                {
                    eb.ToTable("Vehicles");
                    eb.HasKey(v => v.Id);

                    eb.Property(v => v.ModelId).IsRequired();
                    eb.Property(v => v.LicensePlate).HasMaxLength(20).IsRequired();
                    eb.HasIndex(v => v.LicensePlate).IsUnique();
                    eb.Property(v => v.Year).IsRequired();
                    eb.Property(v => v.Color).HasMaxLength(50);
                    eb.Property(v => v.Version).HasMaxLength(50);
                    eb.Property(v => v.Mileage);
                    eb.Property(v => v.Description).HasMaxLength(500);
                    eb.Property(v => v.Transmission).HasConversion<string>().IsRequired();
                    eb.Property(v => v.FuelType).HasConversion<string>().IsRequired();
                    eb.Property(v => v.VehicleType).HasConversion<string>().IsRequired();
                    eb.Property(v => v.Status).HasConversion<string>().IsRequired();
                });
            }
        }
    }

