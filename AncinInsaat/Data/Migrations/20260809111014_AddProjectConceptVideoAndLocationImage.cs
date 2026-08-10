using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectConceptVideoAndLocationImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConceptDescription",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptVideoPath",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptVideoPosterPath",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationImagePath",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConceptDescription",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConceptVideoPath",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConceptVideoPosterPath",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LocationImagePath",
                table: "Projects");
        }
    }
}
