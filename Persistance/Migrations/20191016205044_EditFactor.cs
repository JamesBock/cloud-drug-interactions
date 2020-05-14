using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class EditFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementIngredients_Elements_ElementId",
                table: "ElementIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Elements_ElementId",
                table: "PolicyBase");

            migrationBuilder.RenameColumn(
                name: "ElementId",
                table: "PolicyBase",
                newName: "FactorId");

            migrationBuilder.RenameIndex(
                name: "IX_PolicyBase_ElementId",
                table: "PolicyBase",
                newName: "IX_PolicyBase_FactorId");

            migrationBuilder.RenameColumn(
                name: "ElementUnit",
                table: "Elements",
                newName: "FactorUnit");

            migrationBuilder.RenameColumn(
                name: "ElementType",
                table: "Elements",
                newName: "FactorType");

            migrationBuilder.RenameColumn(
                name: "ElementName",
                table: "Elements",
                newName: "FactorName");

            migrationBuilder.RenameColumn(
                name: "ElementId",
                table: "Elements",
                newName: "FactorId");

            migrationBuilder.RenameColumn(
                name: "ElementId",
                table: "ElementIngredients",
                newName: "FactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementIngredients_Elements_FactorId",
                table: "ElementIngredients",
                column: "FactorId",
                principalTable: "Elements",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Elements_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "Elements",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementIngredients_Elements_FactorId",
                table: "ElementIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Elements_FactorId",
                table: "PolicyBase");

            migrationBuilder.RenameColumn(
                name: "FactorId",
                table: "PolicyBase",
                newName: "ElementId");

            migrationBuilder.RenameIndex(
                name: "IX_PolicyBase_FactorId",
                table: "PolicyBase",
                newName: "IX_PolicyBase_ElementId");

            migrationBuilder.RenameColumn(
                name: "FactorUnit",
                table: "Elements",
                newName: "ElementUnit");

            migrationBuilder.RenameColumn(
                name: "FactorType",
                table: "Elements",
                newName: "ElementType");

            migrationBuilder.RenameColumn(
                name: "FactorName",
                table: "Elements",
                newName: "ElementName");

            migrationBuilder.RenameColumn(
                name: "FactorId",
                table: "Elements",
                newName: "ElementId");

            migrationBuilder.RenameColumn(
                name: "FactorId",
                table: "ElementIngredients",
                newName: "ElementId");

            migrationBuilder.AddForeignKey(
                name: "FK_ElementIngredients_Elements_ElementId",
                table: "ElementIngredients",
                column: "ElementId",
                principalTable: "Elements",
                principalColumn: "ElementId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Elements_ElementId",
                table: "PolicyBase",
                column: "ElementId",
                principalTable: "Elements",
                principalColumn: "ElementId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
