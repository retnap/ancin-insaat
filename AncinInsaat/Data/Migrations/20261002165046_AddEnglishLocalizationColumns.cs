using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnglishLocalizationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FooterTextEn",
                table: "SiteSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkingHoursEn",
                table: "SiteSettings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescriptionEn",
                table: "SeoMetadata",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitleEn",
                table: "SeoMetadata",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AltTextEn",
                table: "ProjectSitePlanImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmenitiesEn",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConceptDescriptionEn",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTypeEn",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescriptionEn",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistanceEn",
                table: "ProjectNearbyPlaces",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ProjectNearbyPlaces",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AltTextEn",
                table: "ProjectImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "ProjectConceptVideos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyebrowEn",
                table: "ProjectConceptVideos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "ProjectConceptVideos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "ProjectConceptImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EyebrowEn",
                table: "ProjectConceptImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "ProjectConceptImages",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "FloorPlanRooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentEn",
                table: "CareerPositions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "CareerPositions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "CareerPositions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FooterTextEn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "WorkingHoursEn",
                table: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "MetaDescriptionEn",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "MetaTitleEn",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "AltTextEn",
                table: "ProjectSitePlanImages");

            migrationBuilder.DropColumn(
                name: "AmenitiesEn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConceptDescriptionEn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectTypeEn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ShortDescriptionEn",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DistanceEn",
                table: "ProjectNearbyPlaces");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ProjectNearbyPlaces");

            migrationBuilder.DropColumn(
                name: "AltTextEn",
                table: "ProjectImages");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "ProjectConceptVideos");

            migrationBuilder.DropColumn(
                name: "EyebrowEn",
                table: "ProjectConceptVideos");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "ProjectConceptVideos");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "ProjectConceptImages");

            migrationBuilder.DropColumn(
                name: "EyebrowEn",
                table: "ProjectConceptImages");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "ProjectConceptImages");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "FloorPlanRooms");

            migrationBuilder.DropColumn(
                name: "DepartmentEn",
                table: "CareerPositions");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "CareerPositions");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "CareerPositions");
        }
    }
}
