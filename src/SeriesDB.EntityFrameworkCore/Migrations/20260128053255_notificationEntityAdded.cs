using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class notificationEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppListasDeSeguimiento_AbpUsers_UsuarioId",
                table: "AppListasDeSeguimiento");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_ListaDeSeguimientoId",
                table: "AppSeries");

            migrationBuilder.DropIndex(
                name: "IX_AppSeries_ListaDeSeguimientoId",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "ListaDeSeguimientoId",
                table: "AppSeries");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "AppListasDeSeguimiento",
                newName: "IdUsuario");

            migrationBuilder.RenameIndex(
                name: "IX_AppListasDeSeguimiento_UsuarioId",
                table: "AppListasDeSeguimiento",
                newName: "IX_AppListasDeSeguimiento_IdUsuario");

            migrationBuilder.CreateTable(
                name: "AppNotificacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppNotificacion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListaSeguimientoSerie",
                columns: table => new
                {
                    ListaDeSeguimientoId = table.Column<int>(type: "int", nullable: false),
                    SerieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaSeguimientoSerie", x => new { x.ListaDeSeguimientoId, x.SerieId });
                    table.ForeignKey(
                        name: "FK_ListaSeguimientoSerie_AppListasDeSeguimiento_ListaDeSeguimientoId",
                        column: x => x.ListaDeSeguimientoId,
                        principalTable: "AppListasDeSeguimiento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaSeguimientoSerie_AppSeries_SerieId",
                        column: x => x.SerieId,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListaSeguimientoSerie_SerieId",
                table: "ListaSeguimientoSerie",
                column: "SerieId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppListasDeSeguimiento_AbpUsers_IdUsuario",
                table: "AppListasDeSeguimiento",
                column: "IdUsuario",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppListasDeSeguimiento_AbpUsers_IdUsuario",
                table: "AppListasDeSeguimiento");

            migrationBuilder.DropTable(
                name: "AppNotificacion");

            migrationBuilder.DropTable(
                name: "ListaSeguimientoSerie");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "AppListasDeSeguimiento",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_AppListasDeSeguimiento_IdUsuario",
                table: "AppListasDeSeguimiento",
                newName: "IX_AppListasDeSeguimiento_UsuarioId");

            migrationBuilder.AddColumn<int>(
                name: "ListaDeSeguimientoId",
                table: "AppSeries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSeries_ListaDeSeguimientoId",
                table: "AppSeries",
                column: "ListaDeSeguimientoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppListasDeSeguimiento_AbpUsers_UsuarioId",
                table: "AppListasDeSeguimiento",
                column: "UsuarioId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSeries_AppListasDeSeguimiento_ListaDeSeguimientoId",
                table: "AppSeries",
                column: "ListaDeSeguimientoId",
                principalTable: "AppListasDeSeguimiento",
                principalColumn: "Id");
        }
    }
}
