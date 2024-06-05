using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class Changefkforanimals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalAnimalCenter");

            migrationBuilder.CreateIndex(
                name: "IX_Animals_AnimalCenterId",
                table: "Animals",
                column: "AnimalCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_AnimalCenters_AnimalCenterId",
                table: "Animals",
                column: "AnimalCenterId",
                principalTable: "AnimalCenters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_AnimalCenters_AnimalCenterId",
                table: "Animals");

            migrationBuilder.DropIndex(
                name: "IX_Animals_AnimalCenterId",
                table: "Animals");

            migrationBuilder.CreateTable(
                name: "AnimalAnimalCenter",
                columns: table => new
                {
                    AnimalCentersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnimalsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalAnimalCenter", x => new { x.AnimalCentersId, x.AnimalsId });
                    table.ForeignKey(
                        name: "FK_AnimalAnimalCenter_AnimalCenters_AnimalCentersId",
                        column: x => x.AnimalCentersId,
                        principalTable: "AnimalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalAnimalCenter_Animals_AnimalsId",
                        column: x => x.AnimalsId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalAnimalCenter_AnimalsId",
                table: "AnimalAnimalCenter",
                column: "AnimalsId");
        }
    }
}
