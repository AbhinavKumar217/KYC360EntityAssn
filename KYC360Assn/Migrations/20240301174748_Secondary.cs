using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KYC360Assn.Migrations
{
    /// <inheritdoc />
    public partial class Secondary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Entities_EntityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Dates_Entities_EntityId",
                table: "Dates");

            migrationBuilder.DropForeignKey(
                name: "FK_Names_Entities_EntityId",
                table: "Names");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Entities_EntityId",
                table: "Addresses",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_Entities_EntityId",
                table: "Dates",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Names_Entities_EntityId",
                table: "Names",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Entities_EntityId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Dates_Entities_EntityId",
                table: "Dates");

            migrationBuilder.DropForeignKey(
                name: "FK_Names_Entities_EntityId",
                table: "Names");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Entities_EntityId",
                table: "Addresses",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dates_Entities_EntityId",
                table: "Dates",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Names_Entities_EntityId",
                table: "Names",
                column: "EntityId",
                principalTable: "Entities",
                principalColumn: "Id");
        }
    }
}
