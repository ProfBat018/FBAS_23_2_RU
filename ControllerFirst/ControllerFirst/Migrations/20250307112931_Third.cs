using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControllerFirst.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserRoles__userN__2A4B4B5E",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Users__66DCF95D7E0943AE",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "userNameRef",
                table: "UserRoles",
                type: "uniqueidentifier",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__C927F7B4D6A3D3A3",
                table: "Users",
                column: "userName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__UserRoles__userN__2A4B4B5E",
                table: "UserRoles",
                column: "userNameRef",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__UserRoles__userN__2A4B4B5E",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UQ__Users__C927F7B4D6A3D3A3",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "userNameRef",
                table: "UserRoles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK__Users__66DCF95D7E0943AE",
                table: "Users",
                column: "userName");

            migrationBuilder.AddForeignKey(
                name: "FK__UserRoles__userN__2A4B4B5E",
                table: "UserRoles",
                column: "userNameRef",
                principalTable: "Users",
                principalColumn: "userName");
        }
    }
}
