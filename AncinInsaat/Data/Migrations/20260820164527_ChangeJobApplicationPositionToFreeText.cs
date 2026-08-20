using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AncinInsaat.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeJobApplicationPositionToFreeText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_CareerPositions_CareerPositionId",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_CareerPositionId",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "CareerPositionId",
                table: "JobApplications");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "JobApplications",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "JobApplications");

            migrationBuilder.AddColumn<int>(
                name: "CareerPositionId",
                table: "JobApplications",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_CareerPositionId",
                table: "JobApplications",
                column: "CareerPositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_CareerPositions_CareerPositionId",
                table: "JobApplications",
                column: "CareerPositionId",
                principalTable: "CareerPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
