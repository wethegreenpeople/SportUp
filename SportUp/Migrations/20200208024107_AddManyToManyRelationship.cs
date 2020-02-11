using Microsoft.EntityFrameworkCore.Migrations;

namespace SportUp.Migrations
{
    public partial class AddManyToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sports_AspNetUsers_SportUpUserId",
                table: "Sports");

            migrationBuilder.DropIndex(
                name: "IX_Sports_SportUpUserId",
                table: "Sports");

            migrationBuilder.DropColumn(
                name: "SportUpUserId",
                table: "Sports");

            migrationBuilder.CreateTable(
                name: "UserSport",
                columns: table => new
                {
                    SportUpUserId = table.Column<string>(nullable: false),
                    SportId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSport", x => new { x.SportId, x.SportUpUserId });
                    table.ForeignKey(
                        name: "FK_UserSport_Sports_SportId",
                        column: x => x.SportId,
                        principalTable: "Sports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSport_AspNetUsers_SportUpUserId",
                        column: x => x.SportUpUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSport_SportUpUserId",
                table: "UserSport",
                column: "SportUpUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSport");

            migrationBuilder.AddColumn<string>(
                name: "SportUpUserId",
                table: "Sports",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sports_SportUpUserId",
                table: "Sports",
                column: "SportUpUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sports_AspNetUsers_SportUpUserId",
                table: "Sports",
                column: "SportUpUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
