using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class OrderItem_AlterTable_AddedFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DesignId",
                table: "OrderItem",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_DesignId",
                table: "OrderItem",
                column: "DesignId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Design_DesignId",
                table: "OrderItem",
                column: "DesignId",
                principalTable: "Design",
                principalColumn: "DesignId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Design_DesignId",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_DesignId",
                table: "OrderItem");

            migrationBuilder.AlterColumn<int>(
                name: "DesignId",
                table: "OrderItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
