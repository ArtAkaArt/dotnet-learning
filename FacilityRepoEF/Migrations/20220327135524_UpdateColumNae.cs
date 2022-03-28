using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacilityContextLib.Migrations
{
    public partial class UpdateColumNae : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_Units_Unitid",
                table: "Tanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Factories_Factoryid",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "Factoryid",
                table: "Units",
                newName: "FactoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Units_Factoryid",
                table: "Units",
                newName: "IX_Units_FactoryId");

            migrationBuilder.RenameColumn(
                name: "Unitid",
                table: "Tanks",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Tanks_Unitid",
                table: "Tanks",
                newName: "IX_Tanks_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_Units_UnitId",
                table: "Tanks",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Factories_FactoryId",
                table: "Units",
                column: "FactoryId",
                principalTable: "Factories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tanks_Units_UnitId",
                table: "Tanks");

            migrationBuilder.DropForeignKey(
                name: "FK_Units_Factories_FactoryId",
                table: "Units");

            migrationBuilder.RenameColumn(
                name: "FactoryId",
                table: "Units",
                newName: "Factoryid");

            migrationBuilder.RenameIndex(
                name: "IX_Units_FactoryId",
                table: "Units",
                newName: "IX_Units_Factoryid");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Tanks",
                newName: "Unitid");

            migrationBuilder.RenameIndex(
                name: "IX_Tanks_UnitId",
                table: "Tanks",
                newName: "IX_Tanks_Unitid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tanks_Units_Unitid",
                table: "Tanks",
                column: "Unitid",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Units_Factories_Factoryid",
                table: "Units",
                column: "Factoryid",
                principalTable: "Factories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
