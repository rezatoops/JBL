using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class AddWonderImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BannerWonderImgId",
                table: "ShopDesigns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannerWonderLink",
                table: "ShopDesigns",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerWonderImgId",
                table: "ShopDesigns");

            migrationBuilder.DropColumn(
                name: "BannerWonderLink",
                table: "ShopDesigns");
        }
    }
}
