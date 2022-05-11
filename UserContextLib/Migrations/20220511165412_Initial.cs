using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserContextLib.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Password = table.Column<byte[]>(type: "bytea", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "bytea", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "PasswordSalt", "Role" },
                values: new object[] { 1, "admin", new byte[] { 111, 36, 19, 64, 38, 162, 61, 77, 110, 110, 45, 77, 82, 198, 120, 96, 251, 240, 181, 67, 221, 211, 98, 18, 137, 46, 5, 47, 4, 129, 207, 131, 60, 177, 99, 25, 158, 184, 40, 158, 21, 240, 128, 113, 21, 109, 132, 24, 91, 183, 75, 100, 251, 72, 134, 227, 231, 181, 127, 104, 180, 12, 20, 246 }, new byte[] { 38, 95, 101, 209, 221, 54, 107, 151, 98, 28, 148, 36, 38, 39, 92, 23, 159, 38, 219, 36, 121, 119, 138, 89, 43, 185, 88, 184, 55, 243, 200, 21, 164, 70, 18, 195, 6, 166, 162, 133, 195, 47, 228, 75, 194, 107, 142, 53, 244, 60, 230, 140, 147, 205, 11, 202, 253, 242, 111, 81, 231, 245, 148, 130, 54, 187, 41, 37, 53, 145, 133, 74, 132, 51, 16, 251, 230, 220, 143, 63, 26, 187, 50, 71, 97, 68, 98, 31, 13, 132, 58, 87, 39, 132, 171, 203, 47, 161, 152, 147, 251, 52, 202, 242, 67, 254, 69, 95, 63, 102, 197, 227, 183, 110, 22, 140, 156, 104, 224, 96, 178, 31, 6, 255, 130, 168, 112, 235 }, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
