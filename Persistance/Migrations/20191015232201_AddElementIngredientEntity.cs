using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class AddElementIngredientEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "PolicyBase",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "Elements",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IngredientName = table.Column<string>(nullable: true),
                    IngredientUnit = table.Column<string>(nullable: true),
                    Osmolarity = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                });

            migrationBuilder.CreateTable(
                name: "ElementIngredients",
                columns: table => new
                {
                    ElementId = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElementIngredients", x => new { x.ElementId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_ElementIngredients_Elements_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ElementIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyBase_IngredientId",
                table: "PolicyBase",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Elements_IngredientId",
                table: "Elements",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_ElementIngredients_IngredientId",
                table: "ElementIngredients",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Ingredients_IngredientId",
                table: "Elements",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Ingredients_IngredientId",
                table: "PolicyBase",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Ingredients_IngredientId",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Ingredients_IngredientId",
                table: "PolicyBase");

            migrationBuilder.DropTable(
                name: "ElementIngredients");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_PolicyBase_IngredientId",
                table: "PolicyBase");

            migrationBuilder.DropIndex(
                name: "IX_Elements_IngredientId",
                table: "Elements");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "PolicyBase");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Elements");
        }
    }
}
