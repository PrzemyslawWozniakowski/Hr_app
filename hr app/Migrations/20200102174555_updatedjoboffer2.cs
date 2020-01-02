using Microsoft.EntityFrameworkCore.Migrations;

namespace hr_app.Migrations
{
    public partial class updatedjoboffer2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Users_UserId",
                table: "JobOffers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Users_UserId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_UserId",
                table: "JobOffers");
        }
    }
}
