using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestBesternoficialya.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFormatoGrupoMySQL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DetallesLogistica",
                table: "Eventos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Agencia",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "Anticipo",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Coordinador",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CortesiasExtras",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CortesiasHabitacion",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoAlimentos",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoHospedaje",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostoPropinas",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cuadruples",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CuentaMaestra",
                table: "Eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CuentasIndividuales",
                table: "Eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Despertador",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Dobles",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DomicilioFacturacion",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Ejecutivo",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Equipaje",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FacturarA",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEntrada",
                table: "Eventos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaGrupo",
                table: "Eventos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaSalida",
                table: "Eventos",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Folio",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Guia",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "HoraLlegada",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NotasAlimentos",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "NumeroReservacion",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Rfc",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RfcFacturacion",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Sencillas",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TelefonoFacturacion",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TipoGrupo",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalEvento",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Triples",
                table: "Eventos",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Agencia",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Anticipo",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Coordinador",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CortesiasExtras",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CortesiasHabitacion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CostoAlimentos",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CostoHospedaje",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CostoPropinas",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Cuadruples",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CuentaMaestra",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CuentasIndividuales",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Despertador",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Dobles",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "DomicilioFacturacion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Ejecutivo",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Equipaje",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "FacturarA",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "FechaEntrada",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "FechaGrupo",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "FechaSalida",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Folio",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Guia",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "HoraLlegada",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NotasAlimentos",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NumeroReservacion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Rfc",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "RfcFacturacion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Sencillas",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TelefonoFacturacion",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TipoGrupo",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TotalEvento",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "Triples",
                table: "Eventos");

            migrationBuilder.UpdateData(
                table: "Eventos",
                keyColumn: "DetallesLogistica",
                keyValue: null,
                column: "DetallesLogistica",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "DetallesLogistica",
                table: "Eventos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
