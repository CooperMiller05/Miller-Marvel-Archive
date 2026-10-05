using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarvelArchive.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterMediaConnectionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterMedia",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    MediaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterMedia", x => new { x.CharacterId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_CharacterMedia_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterMedia_MediaId",
                table: "CharacterMedia",
                column: "MediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterMedia");
        }
    }
}
