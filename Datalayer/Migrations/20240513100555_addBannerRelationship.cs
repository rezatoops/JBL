using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class addBannerRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_Banner1ImgId",
                table: "ShopDesigns",
                column: "Banner1ImgId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_Banner2ImgId",
                table: "ShopDesigns",
                column: "Banner2ImgId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_Banner3ImgId",
                table: "ShopDesigns",
                column: "Banner3ImgId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_Banner4ImgId",
                table: "ShopDesigns",
                column: "Banner4ImgId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Medias_Banner1ImgId",
                table: "ShopDesigns",
                column: "Banner1ImgId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Medias_Banner2ImgId",
                table: "ShopDesigns",
                column: "Banner2ImgId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Medias_Banner3ImgId",
                table: "ShopDesigns",
                column: "Banner3ImgId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Medias_Banner4ImgId",
                table: "ShopDesigns",
                column: "Banner4ImgId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Medias_Banner1ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Medias_Banner2ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Medias_Banner3ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Medias_Banner4ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_Banner1ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_Banner2ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_Banner3ImgId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_Banner4ImgId",
                table: "ShopDesigns");
        }
    }
}
