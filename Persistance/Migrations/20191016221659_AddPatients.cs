using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class AddPatients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ElementIngredients_Elements_FactorId",
                table: "ElementIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ElementIngredients_Ingredients_IngredientId",
                table: "ElementIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Elements_Ingredients_IngredientId",
                table: "Elements");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_User_PrescriberId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Elements_FactorId",
                table: "PolicyBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Elements",
                table: "Elements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ElementIngredients",
                table: "ElementIngredients");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Elements",
                newName: "Factors");

            migrationBuilder.RenameTable(
                name: "ElementIngredients",
                newName: "FactorIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_Elements_IngredientId",
                table: "Factors",
                newName: "IX_Factors_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_ElementIngredients_IngredientId",
                table: "FactorIngredients",
                newName: "IX_FactorIngredients_IngredientId");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Factors",
                table: "Factors",
                column: "FactorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FactorIngredients",
                table: "FactorIngredients",
                columns: new[] { "FactorId", "IngredientId" });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DayOfLife = table.Column<decimal>(nullable: false),
                    WeightG = table.Column<decimal>(nullable: false),
                    PatientType = table.Column<string>(nullable: true),
                    PractitionerUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_Users_PractitionerUserId",
                        column: x => x.PractitionerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PatientId",
                table: "Orders",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PractitionerUserId",
                table: "Patients",
                column: "PractitionerUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_Factors_FactorId",
                table: "FactorIngredients",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_Ingredients_IngredientId",
                table: "FactorIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Ingredients_IngredientId",
                table: "Factors",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Patients_PatientId",
                table: "Orders",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_PrescriberId",
                table: "Orders",
                column: "PrescriberId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_Factors_FactorId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_Ingredients_IngredientId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Ingredients_IngredientId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Patients_PatientId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_PrescriberId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PatientId",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Factors",
                table: "Factors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FactorIngredients",
                table: "FactorIngredients");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Factors",
                newName: "Elements");

            migrationBuilder.RenameTable(
                name: "FactorIngredients",
                newName: "ElementIngredients");

            migrationBuilder.RenameIndex(
                name: "IX_Factors_IngredientId",
                table: "Elements",
                newName: "IX_Elements_IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_FactorIngredients_IngredientId",
                table: "ElementIngredients",
                newName: "IX_ElementIngredients_IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Elements",
                table: "Elements",
                column: "FactorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ElementIngredients",
                table: "ElementIngredients",
                columns: new[] { "FactorId", "IngredientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ElementIngredients_Elements_FactorId",
                table: "ElementIngredients",
                column: "FactorId",
                principalTable: "Elements",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ElementIngredients_Ingredients_IngredientId",
                table: "ElementIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Elements_Ingredients_IngredientId",
                table: "Elements",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_User_PrescriberId",
                table: "Orders",
                column: "PrescriberId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Elements_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "Elements",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
