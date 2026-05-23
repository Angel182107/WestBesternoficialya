using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestBesternoficialya.Migrations
{
    /// <inheritdoc />
    public partial class AgregarHabitaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Precio",
                table: "Habitaciones",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "Habitaciones",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Precio",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "Habitaciones");
        }
    }
}
