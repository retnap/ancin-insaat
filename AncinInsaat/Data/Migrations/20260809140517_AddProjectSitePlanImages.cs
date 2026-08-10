using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSitePlanImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CatalogueComingSoon",
                table: "Projects",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProjectConceptImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    Eyebrow = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectConceptImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectConceptImages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSitePlanImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", nullable: false),
                    AltText = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSitePlanImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSitePlanImages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectConceptImages_ProjectId_DisplayOrder",
                table: "ProjectConceptImages",
                columns: new[] { "ProjectId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSitePlanImages_ProjectId_DisplayOrder",
                table: "ProjectSitePlanImages",
                columns: new[] { "ProjectId", "DisplayOrder" });

            // Backfill existing single-path site plans (Nysa Gold Residence,
            // Davutlar D Latis) into the new table before dropping the old
            // column, so their Hero "Vaziyet Planı" button keeps working
            // unchanged (Vaziyet Planı/Concept/Gallery phase, 2026-08-09).
            migrationBuilder.Sql(@"
                INSERT INTO ProjectSitePlanImages (ProjectId, ImagePath, AltText, DisplayOrder)
                SELECT Id, SitePlanImagePath, '', 1
                FROM Projects
                WHERE SitePlanImagePath IS NOT NULL AND SitePlanImagePath <> '';
            ");

            migrationBuilder.DropColumn(
                name: "SitePlanImagePath",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SitePlanImagePath",
                table: "Projects",
                type: "TEXT",
                nullable: true);

            // Best-effort reverse backfill — restores each project's first
            // (DisplayOrder = 1) site plan image path back onto the
            // single-column shape.
            migrationBuilder.Sql(@"
                UPDATE Projects
                SET SitePlanImagePath = (
                    SELECT ImagePath
                    FROM ProjectSitePlanImages
                    WHERE ProjectSitePlanImages.ProjectId = Projects.Id
                    ORDER BY DisplayOrder
                    LIMIT 1
                );
            ");

            migrationBuilder.DropTable(
                name: "ProjectConceptImages");

            migrationBuilder.DropTable(
                name: "ProjectSitePlanImages");

            migrationBuilder.DropColumn(
                name: "CatalogueComingSoon",
                table: "Projects");
        }
    }
}
