using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControllerFirst.Migrations
{
    /// <inheritdoc />
    public partial class CardTypeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddress_Users_UserId",
                table: "UserAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCard_Users_UserId",
                table: "UserCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCard",
                table: "UserCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress");

            migrationBuilder.RenameTable(
                name: "UserCard",
                newName: "UserCards");

            migrationBuilder.RenameTable(
                name: "UserAddress",
                newName: "UserAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_UserCard_UserId",
                table: "UserCards",
                newName: "IX_UserCards_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddress_UserId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.AddColumn<int>(
                name: "CardType",
                table: "UserCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCards_Users_UserId",
                table: "UserCards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCards_Users_UserId",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCards",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "CardType",
                table: "UserCards");

            migrationBuilder.RenameTable(
                name: "UserCards",
                newName: "UserCard");

            migrationBuilder.RenameTable(
                name: "UserAddresses",
                newName: "UserAddress");

            migrationBuilder.RenameIndex(
                name: "IX_UserCards_UserId",
                table: "UserCard",
                newName: "IX_UserCard_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                table: "UserAddress",
                newName: "IX_UserAddress_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCard",
                table: "UserCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddress",
                table: "UserAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddress_Users_UserId",
                table: "UserAddress",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCard_Users_UserId",
                table: "UserCard",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
