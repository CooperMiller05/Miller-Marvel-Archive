using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelArchive.Migrations
{
    /// <inheritdoc />
    public partial class AddedComicModels2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComicSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicSeries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComicIssues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueNumber = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Creators = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ComicSeriesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicIssues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicIssues_ComicSeries_ComicSeriesId",
                        column: x => x.ComicSeriesId,
                        principalTable: "ComicSeries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComicIssues_ComicSeriesId",
                table: "ComicIssues",
                column: "ComicSeriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicIssues");

            migrationBuilder.DropTable(
                name: "ComicSeries");
        }
    }
}
