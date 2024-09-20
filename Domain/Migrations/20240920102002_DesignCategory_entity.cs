using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class DesignCategory_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Design");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Design");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Design");

            migrationBuilder.AddColumn<int>(
                name: "DesignCategoryId",
                table: "Design",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Design",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MockUrl",
                table: "Design",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesignCategoryId",
                table: "Design");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Design");

            migrationBuilder.DropColumn(
                name: "MockUrl",
                table: "Design");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Design",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Design",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Design",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
