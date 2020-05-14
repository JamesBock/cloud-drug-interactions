using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class DecoratorTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_Factors_FactorId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Ingredients_IngredientId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Patients_PatientId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Factors",
                table: "Factors");

            migrationBuilder.RenameTable(
                name: "Factors",
                newName: "FactorBase");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_PatientId",
                table: "FactorBase",
                newName: "IX_FactorBase_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_IngredientId",
                table: "FactorBase",
                newName: "IX_FactorBase_IngredientId");

            migrationBuilder.AddColumn<int>(
                name: "FactorDecoratorFactorId",
                table: "FactorIngredients",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountContained",
                table: "FactorBase",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOrdered",
                table: "FactorBase",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FactorBase",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "FactorBase",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorBase",
                table: "FactorBase",
                column: "FactorId");

            migrationBuilder.CreateIndex(
                name: "IX_FactorIngredients_FactorDecoratorFactorId",
                table: "FactorIngredients",
                column: "FactorDecoratorFactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorBase_Patients_PatientId",
                table: "FactorBase",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorBase_Ingredients_IngredientId",
                table: "FactorBase",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_FactorBase_FactorDecoratorFactorId",
                table: "FactorIngredients",
                column: "FactorDecoratorFactorId",
                principalTable: "FactorBase",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_FactorBase_FactorId",
                table: "FactorIngredients",
                column: "FactorId",
                principalTable: "FactorBase",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_FactorBase_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "FactorBase",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorBase_Patients_PatientId",
                table: "FactorBase");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorBase_Ingredients_IngredientId",
                table: "FactorBase");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_FactorBase_FactorDecoratorFactorId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_FactorBase_FactorId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_FactorBase_FactorId",
                table: "PolicyBase");

            migrationBuilder.DropIndex(
                name: "IX_FactorIngredients_FactorDecoratorFactorId",
                table: "FactorIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorBase",
                table: "FactorBase");

            migrationBuilder.DropColumn(
                name: "FactorDecoratorFactorId",
                table: "FactorIngredients");

            migrationBuilder.DropColumn(
                name: "AmountContained",
                table: "FactorBase");

            migrationBuilder.DropColumn(
                name: "AmountOrdered",
                table: "FactorBase");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FactorBase");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "FactorBase");

            migrationBuilder.RenameTable(
                name: "FactorBase",
                newName: "Factors");

            migrationBuilder.RenameIndex(
                name: "IX_FactorBase_IngredientId",
                table: "Factors",
                newName: "IX_Factors_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_FactorBase_PatientId",
                table: "Factors",
                newName: "IX_Factors_PatientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Factors",
                table: "Factors",
                column: "FactorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_Factors_FactorId",
                table: "FactorIngredients",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Ingredients_IngredientId",
                table: "Factors",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Patients_PatientId",
                table: "Factors",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
