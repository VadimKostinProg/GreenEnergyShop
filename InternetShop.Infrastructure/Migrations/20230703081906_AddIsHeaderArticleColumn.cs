using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsHeaderArticleColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHeaderArticle",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHeaderArticle",
                table: "Articles");
        }
    }
}
