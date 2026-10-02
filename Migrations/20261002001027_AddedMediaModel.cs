using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelArchive.Migrations
{
    /// <inheritdoc />
    public partial class AddedMediaModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Movies_MovieId",
                table: "Characters");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Characters_MovieId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "publisher",
                table: "Characters",
                newName: "Publisher");

            migrationBuilder.RenameColumn(
                name: "PlaceOfBirth",
                table: "Characters",
                newName: "PlaceOfOrigin");

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Phase = table.Column<int>(type: "int", nullable: false),
                    Saga = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    MCUTime = table.Column<DateOnly>(type: "date", nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Runtime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Director = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distribution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrailerURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IMDbRating = table.Column<double>(type: "float", nullable: false),
                    IMDbURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seasons = table.Column<int>(type: "int", nullable: true),
                    Episodes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.RenameColumn(
                name: "Publisher",
                table: "Characters",
                newName: "publisher");

            migrationBuilder.RenameColumn(
                name: "PlaceOfOrigin",
                table: "Characters",
                newName: "PlaceOfBirth");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_MovieId",
                table: "Characters",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Movies_MovieId",
                table: "Characters",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
