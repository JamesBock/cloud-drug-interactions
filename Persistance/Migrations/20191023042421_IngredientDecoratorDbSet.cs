using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class IngredientDecoratorDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientDecoratorIngredientId",
                table: "IngredientOrders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Osmolarity",
                table: "IngredientBase",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientDecoratorIngredientId",
                table: "FactorIngredients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IngredientOrders_IngredientDecoratorIngredientId",
                table: "IngredientOrders",
                column: "IngredientDecoratorIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorIngredients_IngredientDecoratorIngredientId",
                table: "FactorIngredients",
                column: "IngredientDecoratorIngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_IngredientBase_IngredientDecoratorIngredientId",
                table: "FactorIngredients",
                column: "IngredientDecoratorIngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientOrders_IngredientBase_IngredientDecoratorIngredientId",
                table: "IngredientOrders",
                column: "IngredientDecoratorIngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_IngredientBase_IngredientDecoratorIngredientId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientOrders_IngredientBase_IngredientDecoratorIngredientId",
                table: "IngredientOrders");

            migrationBuilder.DropIndex(
                name: "IX_IngredientOrders_IngredientDecoratorIngredientId",
                table: "IngredientOrders");

            migrationBuilder.DropIndex(
                name: "IX_FactorIngredients_IngredientDecoratorIngredientId",
                table: "FactorIngredients");

            migrationBuilder.DropColumn(
                name: "IngredientDecoratorIngredientId",
                table: "IngredientOrders");

            migrationBuilder.DropColumn(
                name: "Osmolarity",
                table: "IngredientBase");

            migrationBuilder.DropColumn(
                name: "IngredientDecoratorIngredientId",
                table: "FactorIngredients");
        }
    }
}
