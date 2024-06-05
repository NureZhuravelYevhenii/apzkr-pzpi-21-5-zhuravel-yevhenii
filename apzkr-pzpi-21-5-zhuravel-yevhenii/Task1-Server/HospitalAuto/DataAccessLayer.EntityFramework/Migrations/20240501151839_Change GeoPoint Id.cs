using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGeoPointId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoPoints",
                table: "GeoPoints");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GeoPoints",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoPoints",
                table: "GeoPoints",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GeoPoints_FeederId",
                table: "GeoPoints",
                column: "FeederId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoPoints",
                table: "GeoPoints");

            migrationBuilder.DropIndex(
                name: "IX_GeoPoints_FeederId",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GeoPoints");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoPoints",
                table: "GeoPoints",
                column: "FeederId");
        }
    }
}
