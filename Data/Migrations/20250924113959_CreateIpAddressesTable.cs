using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesArchive.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIpAddressesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IpAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    IsBanned = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IpAddressUser",
                columns: table => new
                {
                    IpAddressesId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsersId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpAddressUser", x => new { x.IpAddressesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_IpAddressUser_IpAddress_IpAddressesId",
                        column: x => x.IpAddressesId,
                        principalTable: "IpAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IpAddressUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IpAddressUser_UsersId",
                table: "IpAddressUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IpAddressUser");

            migrationBuilder.DropTable(
                name: "IpAddress");
        }
    }
}
