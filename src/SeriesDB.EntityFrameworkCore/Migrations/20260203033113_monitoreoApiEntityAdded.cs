using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class monitoreoApiEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppMonitoreoApi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraAcceso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TiempoRespuesta = table.Column<float>(type: "real", nullable: false),
                    Errores = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMonitoreoApi", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppMonitoreoApi");
        }
    }
}
