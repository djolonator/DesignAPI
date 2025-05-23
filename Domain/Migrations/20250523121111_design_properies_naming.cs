using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class design_properies_naming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MockUrl",
                table: "Design",
                newName: "PrintImgUrl");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "Design",
                newName: "MockImgUrl");

            migrationBuilder.RenameColumn(
                name: "ImgForPrintUrl",
                table: "Design",
                newName: "LowResImgUrl");

            migrationBuilder.AddColumn<string>(
                name: "BfImgUrl",
                table: "Design",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BfImgUrl",
                table: "Design");

            migrationBuilder.RenameColumn(
                name: "PrintImgUrl",
                table: "Design",
                newName: "MockUrl");

            migrationBuilder.RenameColumn(
                name: "MockImgUrl",
                table: "Design",
                newName: "ImgUrl");

            migrationBuilder.RenameColumn(
                name: "LowResImgUrl",
                table: "Design",
                newName: "ImgForPrintUrl");
        }
    }
}
