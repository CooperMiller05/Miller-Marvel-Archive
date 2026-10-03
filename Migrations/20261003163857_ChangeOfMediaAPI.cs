using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelArchive.Migrations
{
    /// <inheritdoc />
    public partial class ChangeOfMediaAPI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cast",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Distribution",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IMDbRating",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IMDbURL",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "TrailerURL",
                table: "Media",
                newName: "TrailerUrl");

            migrationBuilder.RenameColumn(
                name: "ImageURL",
                table: "Media",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Seasons",
                table: "Media",
                newName: "Season");

            migrationBuilder.RenameColumn(
                name: "Runtime",
                table: "Media",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MCUTime",
                table: "Media",
                newName: "Studio");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Media",
                newName: "Duration");

            migrationBuilder.AlterColumn<string>(
                name: "Saga",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Phase",
                table: "Media",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "BoxOffice",
                table: "Media",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Chronology",
                table: "Media",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMCU",
                table: "Media",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MultiverseDesignation",
                table: "Media",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxOffice",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "Chronology",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "IsMCU",
                table: "Media");

            migrationBuilder.DropColumn(
                name: "MultiverseDesignation",
                table: "Media");

            migrationBuilder.RenameColumn(
                name: "TrailerUrl",
                table: "Media",
                newName: "TrailerURL");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Media",
                newName: "ImageURL");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Media",
                newName: "Runtime");

            migrationBuilder.RenameColumn(
                name: "Studio",
                table: "Media",
                newName: "MCUTime");

            migrationBuilder.RenameColumn(
                name: "Season",
                table: "Media",
                newName: "Seasons");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Media",
                newName: "CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Saga",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Phase",
                table: "Media",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cast",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Distribution",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "IMDbRating",
                table: "Media",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "IMDbURL",
                table: "Media",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
