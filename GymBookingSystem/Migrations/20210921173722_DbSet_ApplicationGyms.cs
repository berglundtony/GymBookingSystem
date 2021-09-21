using Microsoft.EntityFrameworkCore.Migrations;

namespace GymBookingSystem.Migrations
{
    public partial class DbSet_ApplicationGyms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserGymClass_GymClass_GymClassId",
                table: "ApplicationUserGymClass");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass");

            migrationBuilder.RenameTable(
                name: "ApplicationUserGymClass",
                newName: "ApplicationGyms");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserGymClass_GymClassId",
                table: "ApplicationGyms",
                newName: "IX_ApplicationGyms_GymClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationGyms",
                table: "ApplicationGyms",
                columns: new[] { "ApplicationUserId", "GymClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationGyms_AspNetUsers_ApplicationUserId",
                table: "ApplicationGyms",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationGyms_GymClass_GymClassId",
                table: "ApplicationGyms",
                column: "GymClassId",
                principalTable: "GymClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationGyms_AspNetUsers_ApplicationUserId",
                table: "ApplicationGyms");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationGyms_GymClass_GymClassId",
                table: "ApplicationGyms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationGyms",
                table: "ApplicationGyms");

            migrationBuilder.RenameTable(
                name: "ApplicationGyms",
                newName: "ApplicationUserGymClass");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationGyms_GymClassId",
                table: "ApplicationUserGymClass",
                newName: "IX_ApplicationUserGymClass_GymClassId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserGymClass",
                table: "ApplicationUserGymClass",
                columns: new[] { "ApplicationUserId", "GymClassId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_AspNetUsers_ApplicationUserId",
                table: "ApplicationUserGymClass",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserGymClass_GymClass_GymClassId",
                table: "ApplicationUserGymClass",
                column: "GymClassId",
                principalTable: "GymClass",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
