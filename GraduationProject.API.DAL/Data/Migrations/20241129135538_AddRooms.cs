using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.API.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_AspNetUsers_ApplicationUserId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_ApplicationUserId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "Curency",
                table: "Apartments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Apartments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "NumOfRooms",
                table: "Apartments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_AspNetUsers_UserId",
                table: "Apartments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_AspNetUsers_UserId",
                table: "Apartments");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_UserId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "NumOfRooms",
                table: "Apartments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Apartments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Curency",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_ApplicationUserId",
                table: "Apartments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_AspNetUsers_ApplicationUserId",
                table: "Apartments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
