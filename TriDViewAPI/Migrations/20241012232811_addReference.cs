using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TriDViewAPI.Migrations
{
    /// <inheritdoc />
    public partial class addReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanID",
                table: "Stores",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_PlanID",
                table: "Stores",
                column: "PlanID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Plans_PlanID",
                table: "Stores",
                column: "PlanID",
                principalTable: "Plans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Plans_PlanID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_PlanID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "PlanID",
                table: "Stores");
        }
    }
}
