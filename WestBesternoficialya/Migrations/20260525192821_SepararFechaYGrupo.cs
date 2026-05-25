using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestBesternoficialya.Migrations
{
    /// <inheritdoc />
    public partial class SepararFechaYGrupo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaGrupo",
                table: "Eventos",
                newName: "Fecha");

            migrationBuilder.AddColumn<string>(
                name: "Grupo",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grupo",
                table: "Eventos");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "Eventos",
                newName: "FechaGrupo");
        }
    }
}
