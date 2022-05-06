using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserContextLib.Migrations
{
    public partial class InitialCreate : Migration
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
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Login", "Password", "PasswordSalt" },
                values: new object[] { 1, null, "admin", new byte[] { 172, 76, 205, 161, 143, 178, 86, 99, 27, 62, 71, 233, 189, 216, 158, 204, 201, 121, 4, 121, 104, 250, 44, 74, 191, 31, 220, 74, 133, 234, 131, 40, 174, 169, 101, 175, 212, 137, 149, 221, 32, 165, 186, 169, 176, 214, 236, 168, 147, 11, 252, 216, 1, 10, 72, 113, 46, 224, 126, 230, 137, 240, 250, 86 }, new byte[] { 115, 145, 230, 163, 79, 46, 133, 14, 55, 21, 138, 187, 148, 211, 122, 63, 207, 213, 30, 33, 236, 152, 53, 83, 207, 75, 205, 171, 243, 222, 170, 162, 78, 44, 110, 35, 157, 158, 72, 135, 159, 95, 49, 14, 190, 214, 159, 0, 201, 205, 123, 14, 148, 146, 44, 178, 164, 152, 172, 216, 142, 20, 36, 190, 67, 151, 101, 93, 179, 157, 238, 166, 153, 127, 132, 65, 20, 185, 102, 177, 190, 193, 133, 104, 170, 188, 111, 161, 57, 70, 95, 5, 8, 36, 7, 68, 250, 242, 200, 29, 237, 46, 41, 244, 90, 183, 240, 24, 103, 236, 86, 234, 119, 16, 209, 212, 40, 102, 83, 59, 71, 182, 114, 214, 11, 34, 213, 18 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
