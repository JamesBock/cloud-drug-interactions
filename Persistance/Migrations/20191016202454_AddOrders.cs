using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class AddOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PolicyBase",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Ingredients",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PostNominals = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderName = table.Column<string>(nullable: true),
                    OrderDescription = table.Column<string>(nullable: true),
                    PrescriberNotes = table.Column<string>(nullable: true),
                    PrescriberId = table.Column<int>(nullable: false),
                    TimeOrdered = table.Column<DateTime>(nullable: false),
                    TimeExecuted = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Volume = table.Column<double>(nullable: true),
                    Duration = table.Column<long>(nullable: true),
                    AdimnistrationRoute = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_User_PrescriberId",
                        column: x => x.PrescriberId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngredientOrders",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false),
                    IngredientId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientOrders", x => new { x.OrderId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_IngredientOrders_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyBase_OrderId",
                table: "PolicyBase",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_OrderId",
                table: "Ingredients",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientOrders_IngredientId",
                table: "IngredientOrders",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PrescriberId",
                table: "Orders",
                column: "PrescriberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Orders_OrderId",
                table: "Ingredients",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Orders_OrderId",
                table: "PolicyBase",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Orders_OrderId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Orders_OrderId",
                table: "PolicyBase");

            migrationBuilder.DropTable(
                name: "IngredientOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_PolicyBase_OrderId",
                table: "PolicyBase");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_OrderId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PolicyBase");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Ingredients");
        }
    }
}
