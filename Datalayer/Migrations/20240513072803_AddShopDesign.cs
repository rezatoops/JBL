using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class AddShopDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopDesigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CatId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Banner1Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Banner2Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Banner3Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Banner4Link = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Banner1ImgId = table.Column<int>(type: "int", nullable: false),
                    Banner2ImgId = table.Column<int>(type: "int", nullable: false),
                    Banner3ImgId = table.Column<int>(type: "int", nullable: false),
                    Banner4ImgId = table.Column<int>(type: "int", nullable: false),
                    NumberOfProduct = table.Column<int>(type: "int", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopDesigns", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopDesigns");
        }
    }
}
