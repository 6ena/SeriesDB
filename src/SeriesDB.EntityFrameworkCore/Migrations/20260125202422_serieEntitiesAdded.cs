using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeriesDB.Migrations
{
    /// <inheritdoc />
    public partial class serieEntitiesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genero",
                table: "AppSeries",
                newName: "Tipo");

            migrationBuilder.AddColumn<string>(
                name: "Actores",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Clasificacion",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Directores",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Duracion",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Escritores",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FechaEstreno",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Generos",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Idiomas",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImdbCalificacion",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImdbId",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ImdbVotos",
                table: "AppSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Pais",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Poster",
                table: "AppSeries",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sinopsis",
                table: "AppSeries",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalTemporadas",
                table: "AppSeries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppTemporadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroTemporada = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FechaLanzamiento = table.Column<DateOnly>(type: "date", maxLength: 128, nullable: false),
                    SerieID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTemporadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTemporadas_AppSeries_SerieID",
                        column: x => x.SerieID,
                        principalTable: "AppSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppEpisodios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NroEpisodio = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Duracion = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Resumen = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    FechaEstreno = table.Column<DateOnly>(type: "date", nullable: false),
                    Directores = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Escritores = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TemporadaID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppEpisodios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppEpisodios_AppTemporadas_TemporadaID",
                        column: x => x.TemporadaID,
                        principalTable: "AppTemporadas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppEpisodios_TemporadaID",
                table: "AppEpisodios",
                column: "TemporadaID");

            migrationBuilder.CreateIndex(
                name: "IX_AppTemporadas_SerieID",
                table: "AppTemporadas",
                column: "SerieID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppEpisodios");

            migrationBuilder.DropTable(
                name: "AppTemporadas");

            migrationBuilder.DropColumn(
                name: "Actores",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Clasificacion",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Directores",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Duracion",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Escritores",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "FechaEstreno",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Generos",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Idiomas",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "ImdbCalificacion",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "ImdbId",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "ImdbVotos",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Pais",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Poster",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "Sinopsis",
                table: "AppSeries");

            migrationBuilder.DropColumn(
                name: "TotalTemporadas",
                table: "AppSeries");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "AppSeries",
                newName: "Genero");
        }
    }
}
