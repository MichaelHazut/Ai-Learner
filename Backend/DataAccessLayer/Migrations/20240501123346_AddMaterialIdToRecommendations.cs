using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialIdToRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Materials_MaterialId",
                table: "Recommendations");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "Recommendations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Materials_MaterialId",
                table: "Recommendations",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_Materials_MaterialId",
                table: "Recommendations");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "Recommendations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_Materials_MaterialId",
                table: "Recommendations",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId");
        }
    }
}
