using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ETIDrive_Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserFolderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFolders_AspNetUsers_UserId",
                table: "UserFolders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFolders",
                table: "UserFolders");

            migrationBuilder.DropIndex(
                name: "IX_UserFolders_UserId",
                table: "UserFolders");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserFolders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFolders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFolders",
                table: "UserFolders",
                columns: new[] { "UserId", "FolderId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFolders_AspNetUsers_UserId",
                table: "UserFolders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFolders_AspNetUsers_UserId",
                table: "UserFolders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFolders",
                table: "UserFolders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserFolders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserFolders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFolders",
                table: "UserFolders",
                columns: new[] { "Id", "FolderId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFolders_UserId",
                table: "UserFolders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFolders_AspNetUsers_UserId",
                table: "UserFolders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
