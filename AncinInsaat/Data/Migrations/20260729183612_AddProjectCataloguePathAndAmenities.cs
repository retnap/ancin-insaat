using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectCataloguePathAndAmenities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Amenities",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CataloguePath",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CataloguePath",
                table: "Projects");
        }
    }
}
