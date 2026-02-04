using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class configNotificacionfEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfigNotificacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUsuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificacionPantalla = table.Column<bool>(type: "bit", nullable: false),
                    NotificacionEmail = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigNotificacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppConfigNotificacion_AbpUsers_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppConfigNotificacion_IdUsuario",
                table: "AppConfigNotificacion",
                column: "IdUsuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfigNotificacion");
        }
    }
}
