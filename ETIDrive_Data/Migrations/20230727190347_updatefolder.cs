using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETIDrive_Data.Migrations
{
    /// <inheritdoc />
    public partial class Updatefolder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Folders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Folders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                table: "Folders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_CreatedById",
                table: "Folders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Folders_ModifiedById",
                table: "Folders",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_CreatedById",
                table: "Folders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_AspNetUsers_ModifiedById",
                table: "Folders",
                column: "ModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_CreatedById",
                table: "Folders");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_AspNetUsers_ModifiedById",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_CreatedById",
                table: "Folders");

            migrationBuilder.DropIndex(
                name: "IX_Folders_ModifiedById",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Folders");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Folders");
        }
    }
}
