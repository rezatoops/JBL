using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class AddShopDesignRelationWithCat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ShopDesigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_CategoryId",
                table: "ShopDesigns",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Categories_CategoryId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_CategoryId",
                table: "ShopDesigns");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ShopDesigns");
        }
    }
}
