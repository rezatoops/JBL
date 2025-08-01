using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class WonderImgRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShopDesigns_BannerWonderImgId",
                table: "ShopDesigns",
                column: "BannerWonderImgId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopDesigns_Medias_BannerWonderImgId",
                table: "ShopDesigns",
                column: "BannerWonderImgId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopDesigns_Medias_BannerWonderImgId",
                table: "ShopDesigns");

            migrationBuilder.DropIndex(
                name: "IX_ShopDesigns_BannerWonderImgId",
                table: "ShopDesigns");
        }
    }
}
