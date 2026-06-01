using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestBesternoficialya.Migrations
{
    /// <inheritdoc />
    public partial class AgregarFormatoReservacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsUrgente",
                table: "Eventos");

            migrationBuilder.AlterColumn<string>(
                name: "CuentasIndividuales",
                table: "Eventos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "CuentaMaestra",
                table: "Eventos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Anticipo",
                table: "Eventos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Ant",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NochesCortesia",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NochesCuadruple",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NochesDoble",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NochesSencilla",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NochesTriple",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PropinaCortesia",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PropinaCuadruple",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PropinaDoble",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PropinaSencilla",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PropinaTriple",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaCortesia",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaCuadruple",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaDoble",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaSencilla",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TarifaTriple",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormatosReservacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Folio = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaElaboracion = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NombreGrupo = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PersonaQueSolicita = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefono = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FechaSalida = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TotalNoches = table.Column<int>(type: "int", nullable: true),
                    TotalHabitaciones = table.Column<int>(type: "int", nullable: true),
                    CantidadSencilla = table.Column<int>(type: "int", nullable: true),
                    TarifaSencilla = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalSencilla = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CantidadDoble = table.Column<int>(type: "int", nullable: true),
                    TarifaDoble = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalDoble = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CantidadTriple = table.Column<int>(type: "int", nullable: true),
                    TarifaTriple = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalTriple = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CantidadCuadruple = table.Column<int>(type: "int", nullable: true),
                    TarifaCuadruple = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalCuadruple = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Iva = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Ish = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AnticipoRequerido = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    FechaLimiteAnticipo = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PoliticasCancelacion = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Observaciones = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormatosReservacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FormatosReservacion_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FormatosReservacion_EventoId",
                table: "FormatosReservacion",
                column: "EventoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormatosReservacion");

            migrationBuilder.DropColumn(
                name: "Ant",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NochesCortesia",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NochesCuadruple",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NochesDoble",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NochesSencilla",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "NochesTriple",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "PropinaCortesia",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "PropinaCuadruple",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "PropinaDoble",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "PropinaSencilla",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "PropinaTriple",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TarifaCortesia",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TarifaCuadruple",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TarifaDoble",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TarifaSencilla",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "TarifaTriple",
                table: "Eventos");

            migrationBuilder.AlterColumn<bool>(
                name: "CuentasIndividuales",
                table: "Eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "CuentaMaestra",
                table: "Eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "Anticipo",
                table: "Eventos",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "EsUrgente",
                table: "Eventos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
