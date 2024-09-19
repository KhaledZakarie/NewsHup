using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsHup.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CatId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CatId",
                table: "Articles",
                column: "CatId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CatId",
                table: "Articles");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CatId",
                table: "Articles",
                column: "CatId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
