using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMobileSlider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Medias_MediaId",
                table: "Sliders");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "Sliders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MobileMediaId",
                table: "Sliders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sliders_MobileMediaId",
                table: "Sliders",
                column: "MobileMediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Medias_MediaId",
                table: "Sliders",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Medias_MobileMediaId",
                table: "Sliders",
                column: "MobileMediaId",
                principalTable: "Medias",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Medias_MediaId",
                table: "Sliders");

            migrationBuilder.DropForeignKey(
                name: "FK_Sliders_Medias_MobileMediaId",
                table: "Sliders");

            migrationBuilder.DropIndex(
                name: "IX_Sliders_MobileMediaId",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "MobileMediaId",
                table: "Sliders");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "Sliders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sliders_Medias_MediaId",
                table: "Sliders",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
