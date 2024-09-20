using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class Entities_changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DesignCategory",
                columns: table => new
                {
                    DesignCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignCategory", x => x.DesignCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Design_DesignCategoryId",
                table: "Design",
                column: "DesignCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Design_DesignCategory_DesignCategoryId",
                table: "Design",
                column: "DesignCategoryId",
                principalTable: "DesignCategory",
                principalColumn: "DesignCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Design_DesignCategory_DesignCategoryId",
                table: "Design");

            migrationBuilder.DropTable(
                name: "DesignCategory");

            migrationBuilder.DropIndex(
                name: "IX_Design_DesignCategoryId",
                table: "Design");
        }
    }
}
