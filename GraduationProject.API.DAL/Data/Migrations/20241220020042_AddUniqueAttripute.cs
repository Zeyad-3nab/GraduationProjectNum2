using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.API.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueAttripute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Governorate",
                table: "Apartments");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "ApartmentImages",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "WebsiteURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreation",
                table: "Apartments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRent",
                table: "Apartments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreation",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "IsRent",
                table: "Apartments");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ApartmentImages",
                newName: "Url");

            migrationBuilder.AlterColumn<string>(
                name: "WebsiteURL",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Governorate",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
