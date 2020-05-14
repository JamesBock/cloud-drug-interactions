using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Elements",
                columns: table => new
                {
                    ElementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElementType = table.Column<string>(nullable: true),
                    ElementName = table.Column<string>(nullable: true),
                    ElementUnit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elements", x => x.ElementId);
                });

            migrationBuilder.CreateTable(
                name: "PolicyBase",
                columns: table => new
                {
                    PolicyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientType = table.Column<string>(nullable: true),
                    PolicyBasis = table.Column<string>(nullable: true),
                    Minimum = table.Column<decimal>(nullable: false),
                    Maximum = table.Column<decimal>(nullable: false),
                    WarningLevel = table.Column<string>(nullable: true),
                    PolicyDuration = table.Column<long>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    ElementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyBase", x => x.PolicyId);
                    table.ForeignKey(
                        name: "FK_PolicyBase_Elements_ElementId",
                        column: x => x.ElementId,
                        principalTable: "Elements",
                        principalColumn: "ElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyBase_ElementId",
                table: "PolicyBase",
                column: "ElementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyBase");

            migrationBuilder.DropTable(
                name: "Elements");
        }
    }
}
