using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestBesternoficialya.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoCeldasNuevas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estacionamiento",
                table: "NotificacionesEventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Garantia",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Menus",
                table: "NotificacionesEventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NoDias2",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoDias3",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoDias4",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoDias5",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoPax2",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoPax3",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoPax4",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NoPax5",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Preparar",
                table: "NotificacionesEventos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estacionamiento",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "Garantia",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "Menus",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoDias2",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoDias3",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoDias4",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoDias5",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoPax2",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoPax3",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoPax4",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "NoPax5",
                table: "NotificacionesEventos");

            migrationBuilder.DropColumn(
                name: "Preparar",
                table: "NotificacionesEventos");
        }
    }
}
