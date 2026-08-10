using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFloorPlanApartmentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "FloorPlans");

            migrationBuilder.AddColumn<string>(
                name: "ApartmentType",
                table: "FloorPlans",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "GrossAreaM2",
                table: "FloorPlans",
                type: "TEXT",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetAreaM2",
                table: "FloorPlans",
                type: "TEXT",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalesGrossAreaM2",
                table: "FloorPlans",
                type: "TEXT",
                precision: 6,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "FloorPlanRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FloorPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AreaM2 = table.Column<decimal>(type: "TEXT", precision: 6, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FloorPlanRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FloorPlanRooms_FloorPlans_FloorPlanId",
                        column: x => x.FloorPlanId,
                        principalTable: "FloorPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FloorPlanRooms_FloorPlanId",
                table: "FloorPlanRooms",
                column: "FloorPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FloorPlanRooms");

            migrationBuilder.DropColumn(
                name: "ApartmentType",
                table: "FloorPlans");

            migrationBuilder.DropColumn(
                name: "GrossAreaM2",
                table: "FloorPlans");

            migrationBuilder.DropColumn(
                name: "NetAreaM2",
                table: "FloorPlans");

            migrationBuilder.DropColumn(
                name: "SalesGrossAreaM2",
                table: "FloorPlans");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FloorPlans",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
