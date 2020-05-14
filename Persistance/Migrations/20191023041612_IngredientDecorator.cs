using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class IngredientDecorator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorBase_Ingredients_IngredientId",
                table: "FactorBase");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_Ingredients_IngredientId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientOrders_Ingredients_IngredientId",
                table: "IngredientOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Ingredients_IngredientId",
                table: "PolicyBase");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "FactorBase",
                newName: "IngredientBaseIngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_FactorBase_IngredientId",
                table: "FactorBase",
                newName: "IX_FactorBase_IngredientBaseIngredientId");

            migrationBuilder.AddColumn<decimal>(
                name: "PerIngredientUnit",
                table: "FactorBase",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IngredientBase",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IngredientName = table.Column<string>(nullable: true),
                    IngredientUnit = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    OrderId = table.Column<int>(nullable: true),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientBase", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_IngredientBase_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IngredientBase_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientBase_OrderId",
                table: "IngredientBase",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientBase_PatientId",
                table: "IngredientBase",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorBase_IngredientBase_IngredientBaseIngredientId",
                table: "FactorBase",
                column: "IngredientBaseIngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_IngredientBase_IngredientId",
                table: "FactorIngredients",
                column: "IngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientOrders_IngredientBase_IngredientId",
                table: "IngredientOrders",
                column: "IngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_IngredientBase_IngredientId",
                table: "PolicyBase",
                column: "IngredientId",
                principalTable: "IngredientBase",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FactorBase_IngredientBase_IngredientBaseIngredientId",
                table: "FactorBase");

            migrationBuilder.DropForeignKey(
                name: "FK_FactorIngredients_IngredientBase_IngredientId",
                table: "FactorIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientOrders_IngredientBase_IngredientId",
                table: "IngredientOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_IngredientBase_IngredientId",
                table: "PolicyBase");

            migrationBuilder.DropTable(
                name: "IngredientBase");

            migrationBuilder.DropColumn(
                name: "PerIngredientUnit",
                table: "FactorBase");

            migrationBuilder.RenameColumn(
                name: "IngredientBaseIngredientId",
                table: "FactorBase",
                newName: "IngredientId");

            migrationBuilder.RenameIndex(
                name: "IX_FactorBase_IngredientBaseIngredientId",
                table: "FactorBase",
                newName: "IX_FactorBase_IngredientId");

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    IngredientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IngredientName = table.Column<string>(nullable: true),
                    IngredientUnit = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: true),
                    Osmolarity = table.Column<decimal>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_Ingredients_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ingredients_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_OrderId",
                table: "Ingredients",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_PatientId",
                table: "Ingredients",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_FactorBase_Ingredients_IngredientId",
                table: "FactorBase",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FactorIngredients_Ingredients_IngredientId",
                table: "FactorIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientOrders_Ingredients_IngredientId",
                table: "IngredientOrders",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Ingredients_IngredientId",
                table: "PolicyBase",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
