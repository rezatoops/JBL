using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datalayer.Migrations
{
    /// <inheritdoc />
    public partial class editOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VariableId",
                table: "OrderProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_VariableId",
                table: "OrderProducts",
                column: "VariableId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Variables_VariableId",
                table: "OrderProducts",
                column: "VariableId",
                principalTable: "Variables",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Variables_VariableId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_VariableId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "VariableId",
                table: "OrderProducts");
        }
    }
}
