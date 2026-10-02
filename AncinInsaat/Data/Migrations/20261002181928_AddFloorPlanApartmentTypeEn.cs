using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFloorPlanApartmentTypeEn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApartmentTypeEn",
                table: "FloorPlans",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApartmentTypeEn",
                table: "FloorPlans");
        }
    }
}
