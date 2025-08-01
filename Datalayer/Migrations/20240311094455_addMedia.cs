using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class addMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryImage",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryImageId",
                table: "Categories",
                type: "int",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Medias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medias", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryImageId",
                table: "Categories",
                column: "CategoryImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Medias_CategoryImageId",
                table: "Categories",
                column: "CategoryImageId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Medias_CategoryImageId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "Medias");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryImageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryImageId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryImage",
                table: "Categories",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
