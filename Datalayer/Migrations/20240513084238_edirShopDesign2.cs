using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class edirShopDesign2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns");

            migrationBuilder.DropColumn(
                name: "CatId",
                table: "ShopDesigns");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ShopDesigns",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "ShopDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CatId",
                table: "ShopDesigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
