using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UWPLockStep.Persistance.Migrations
{
    public partial class ManualFactorPolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Ingredients",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId",
                table: "Factors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_PatientId",
                table: "Ingredients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_PatientId",
                table: "Factors",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_Patients_PatientId",
                table: "Factors",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_Patients_PatientId",
                table: "Ingredients",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_Patients_PatientId",
                table: "Factors");

            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_Patients_PatientId",
                table: "Ingredients");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_PatientId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Factors_PatientId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Factors");

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyBase_Factors_FactorId",
                table: "PolicyBase",
                column: "FactorId",
                principalTable: "Factors",
                principalColumn: "FactorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
