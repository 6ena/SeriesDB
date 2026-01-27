using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class addedListaSeguimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListaDeSeguimientoId",
                table: "AppSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppListasDeSeguimiento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaModificacion = table.Column<DateOnly>(type: "date", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppListasDeSeguimiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppListasDeSeguimiento_AbpUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSeries_ListaDeSeguimientoId",
                table: "AppSeries",
                column: "ListaDeSeguimientoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppListasDeSeguimiento_UsuarioId",
                table: "AppListasDeSeguimiento",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_ListaDeSeguimientoId",
                table: "AppSeries",
                column: "ListaDeSeguimientoId",
                principalTable: "AppListasDeSeguimiento",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_ListaDeSeguimientoId",
                table: "AppSeries");

            migrationBuilder.DropTable(
                name: "AppListasDeSeguimiento");

            migrationBuilder.DropIndex(
                name: "IX_AppSeries_ListaDeSeguimientoId",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "ListaDeSeguimientoId",
                table: "AppSeries");
        }
    }
}
