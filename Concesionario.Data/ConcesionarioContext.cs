using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.Finance;
using Concesionario.Domain.Entities.SalesReservation;
using Concesionario.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Data
{
   public class ConcesionarioContext : DbContext
    {

        public ConcesionarioContext(DbContextOptions<ConcesionarioContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==============================
            // Configuración de User
            // ==============================
          

            // ==============================
            // Configuración de Employee
            // ==============================
            modelBuilder.Entity<Employee>(eb =>
            {
                eb.ToTable("Employees");

                eb.HasKey(e => e.Id);

               
                // Relación Employee - Branch (N:1)
                eb.HasOne(e => e.Branch)
                    .WithMany()
                    .HasForeignKey(e => e.BranchId)
                    .OnDelete(DeleteBehavior.Restrict);

              
            });

            // ==============================
            // Configuración de Customer
            // ==============================
            modelBuilder.Entity<Customer>(eb =>
            {
                eb.ToTable("Customers");

                eb.HasKey(c => c.Id);

                

                eb.HasMany(c => c.Sales)
                    .WithOne(s => s.Customer)
                    .HasForeignKey(s => s.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.HasMany(c => c.Reservations)
                    .WithOne(r => r.Customer)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ==============================
            // Configuración de Role
            // ==============================
            modelBuilder.Entity<Position>(eb =>
            {
                eb.ToTable("Positions");

                eb.HasKey(r => r.Id);

                eb.Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

                eb.Property(r => r.Descripcion)
                    .HasMaxLength(100)
                    .IsRequired();
            });
            // ==============================
            // Configuración de Reservation
            // ==============================
            modelBuilder.Entity<Reservation>(eb =>
            {
                eb.ToTable("Reservations");

                eb.HasKey(r => r.Id);

                eb.Property(r => r.StartDate)
                    .IsRequired();

                eb.Property(r => r.EndDate)
                    .IsRequired();

                eb.Property(r => r.ReservationAmount)
                    .HasPrecision(15, 2)
                    .IsRequired();

                eb.Property(r => r.Status)
                    .IsRequired();

                // Relación Reservation - Customer (N:1)
                eb.HasOne(r => r.Customer)
                    .WithMany(c => c.Reservations)
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.HasOne<Vehicle>()
                  .WithMany()
                   .HasForeignKey(r => r.VehicleId)
                  .OnDelete(DeleteBehavior.Restrict);
            });

            // ==============================
            // Configuración de Sale
            // ==============================
            modelBuilder.Entity<Sale>(eb =>
            {
                eb.ToTable("Sales");

                eb.HasKey(s => s.Id);

                eb.Property(s => s.SaleDate)
                    .IsRequired();

                eb.Property(s => s.TotalAmount)
                    .HasPrecision(15, 2)
                    .IsRequired();

                eb.Property(s => s.Status)
                    .IsRequired();

                // Relación Sale - Customer (N:1, opcional)
                eb.HasOne(s => s.Customer)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(s => s.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación Sale - Reservation (1:1 o N:1)
                eb.HasOne<Reservation>()
                    .WithMany() // o .WithOne() si quieres 1:1
                    .HasForeignKey(s => s.ReservationId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<FinancingPlan>(eb =>
            {
                eb.ToTable("FinancingPlans");

                eb.HasKey(fp => fp.Id);

                eb.Property(fp => fp.SaleId)
                    .IsRequired();

                eb.Property(fp => fp.TotalAmount)
                    .HasPrecision(15, 2)
                    .IsRequired();

                eb.Property(fp => fp.NumberOfInstallments)
                    .IsRequired();

                eb.Property(fp => fp.InterestRate)
                    .HasPrecision(5, 2)
                    .IsRequired();

                eb.Property(fp => fp.StartDate)
                    .IsRequired();

                // Relación 1:N con Installments
                eb.HasMany(fp => fp.Installments)
                    .WithOne()
                    .HasForeignKey(i => i.FinancingPlanId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Installment>(eb =>
            {
                eb.ToTable("Installments");

                eb.HasKey(i => i.Id);

                eb.Property(i => i.FinancingPlanId)
                    .IsRequired();

                eb.Property(i => i.Number)
                    .IsRequired();

                eb.Property(i => i.Amount)
                    .HasPrecision(15, 2)
                    .IsRequired();

                eb.Property(i => i.DueDate)
                    .IsRequired();

                eb.Property(i => i.Status)
                    .IsRequired();

                // Relación 1:N con Payments
                eb.HasMany(i => i.Payments)
                    .WithOne()
                    .HasForeignKey(p => p.InstallmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Payment>(eb =>
            {
                eb.ToTable("Payments");

                eb.HasKey(p => p.Id);

                eb.Property(p => p.InstallmentId)
                    .IsRequired();

                eb.Property(p => p.PaymentDate)
                    .IsRequired();

                eb.Property(p => p.Amount)
                    .HasPrecision(15, 2)
                    .IsRequired();

                eb.Property(p => p.Method)
                    .IsRequired();

                eb.Property(p => p.Status)
                    .IsRequired();
            });
            modelBuilder.Entity<VehicleBrand>(eb =>
            {
                eb.ToTable("VehicleBrands");

                eb.HasKey(b => b.Id);

                eb.Property(b => b.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                eb.Property(b => b.Country)
                    .HasMaxLength(100)
                    .IsRequired();

                eb.HasMany(b => b.Models)
                    .WithOne(m => m.Brand)
                    .HasForeignKey(m => m.BrandId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<VehicleModel>(eb =>
            {
                eb.ToTable("VehicleModels");

                eb.HasKey(m => m.Id);

                eb.Property(m => m.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                eb.Property(m => m.BrandId)
                    .IsRequired();

                eb.HasOne(m => m.Brand)
                    .WithMany(b => b.Models)
                    .HasForeignKey(m => m.BrandId)
                    .OnDelete(DeleteBehavior.Restrict);

                eb.HasMany(m => m.Vehicles)
                    .WithOne(v => v.Model)
                    .HasForeignKey(v => v.ModelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Vehicle>(eb =>
            {
                eb.ToTable("Vehicles");

                eb.HasKey(v => v.Id);

                eb.Property(v => v.ModelId)
                    .IsRequired();

                eb.Property(v => v.LicensePlate)
                    .HasMaxLength(20)
                    .IsRequired(false);

                eb.Property(v => v.Year)
                    .IsRequired();

                eb.Property(v => v.Color)
                    .HasMaxLength(50)
                    .IsRequired(false);

                eb.Property(v => v.Version)
                    .HasMaxLength(50)
                    .IsRequired(false);

                eb.Property(v => v.Mileage)
                    .IsRequired(false);

                eb.Property(v => v.Description)
                    .HasMaxLength(500)
                    .IsRequired(false);

                eb.Property(v => v.Transmission)
                    .HasConversion<string>()
                    .IsRequired();

                eb.Property(v => v.FuelType)
                    .HasConversion<string>()
                    .IsRequired();

                eb.Property(v => v.VehicleType)
                    .HasConversion<string>()
                    .IsRequired();

                eb.Property(v => v.Status)
                    .HasConversion<string>()
                    .IsRequired();

                eb.HasOne(v => v.Model)
                    .WithMany(m => m.Vehicles)
                    .HasForeignKey(v => v.ModelId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

    }
}
