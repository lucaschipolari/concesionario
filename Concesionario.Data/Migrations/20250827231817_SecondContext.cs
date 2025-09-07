using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concesionario.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PromotionalPrice",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Sales",
                newName: "OtherTaxes");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Vehicles",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "BasePrice",
                table: "Sales",
                type: "decimal(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalPrice",
                table: "Sales",
                type: "decimal(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IVA",
                table: "Sales",
                type: "decimal(15,2)",
                precision: 15,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ReservationId1",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_EmployeeId",
                table: "Sales",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ReservationId1",
                table: "Sales",
                column: "ReservationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_VehicleId",
                table: "Sales",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Employees_EmployeeId",
                table: "Sales",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Reservations_ReservationId1",
                table: "Sales",
                column: "ReservationId1",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Employees_EmployeeId",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Reservations_ReservationId1",
                table: "Sales");

            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Vehicles_VehicleId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_EmployeeId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_ReservationId1",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_VehicleId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "BasePrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "FinalPrice",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IVA",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "ReservationId1",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "OtherTaxes",
                table: "Sales",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Vehicles",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PromotionalPrice",
                table: "Vehicles",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReservationId",
                table: "Sales",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
