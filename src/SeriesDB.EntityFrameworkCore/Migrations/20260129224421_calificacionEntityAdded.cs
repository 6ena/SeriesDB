using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class calificacionEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppListasDeSeguimiento_IdUsuario",
                table: "AppListasDeSeguimiento");

            migrationBuilder.CreateTable(
                name: "AppCalificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroCalificacion = table.Column<float>(type: "real", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerieID = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCalificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCalificacion_AbpUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCalificacion_AppSeries_SerieID",
                        column: x => x.SerieID,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppNotificacion_IdUsuario",
                table: "AppNotificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AppListasDeSeguimiento_IdUsuario",
                table: "AppListasDeSeguimiento",
                column: "IdUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppCalificacion_IdUsuario",
                table: "AppCalificacion",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_AppCalificacion_SerieID",
                table: "AppCalificacion",
                column: "SerieID");

            migrationBuilder.AddForeignKey(
                name: "FK_AppNotificacion_AbpUsers_IdUsuario",
                table: "AppNotificacion",
                column: "IdUsuario",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppNotificacion_AbpUsers_IdUsuario",
                table: "AppNotificacion");

            migrationBuilder.DropTable(
                name: "AppCalificacion");

            migrationBuilder.DropIndex(
                name: "IX_AppNotificacion_IdUsuario",
                table: "AppNotificacion");

            migrationBuilder.DropIndex(
                name: "IX_AppListasDeSeguimiento_IdUsuario",
                table: "AppListasDeSeguimiento");

            migrationBuilder.CreateIndex(
                name: "IX_AppListasDeSeguimiento_IdUsuario",
                table: "AppListasDeSeguimiento",
                column: "IdUsuario");
        }
    }
}
