using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "Points", "UserName" },
                values: new object[,]
                {
                    { new Guid("07954be9-1416-4c31-8aec-4526d785052f"), "user4@example.com", "hashedPassword", 1938005744, "User4" },
                    { new Guid("08b6de12-8c71-494f-8580-bcfa2a3289f5"), "user6@example.com", "hashedPassword", 2037243568, "User6" },
                    { new Guid("1de0ff64-d9d0-4cc4-b53d-7b1231fcdcce"), "user19@example.com", "hashedPassword", 825105703, "User19" },
                    { new Guid("3632ed47-ba65-46d0-b3ba-ec50788933ff"), "user5@example.com", "hashedPassword", 761513014, "User5" },
                    { new Guid("39c4f7a2-6eb4-4716-8b06-c8b9b28fcfe1"), "user12@example.com", "hashedPassword", 1156976611, "User12" },
                    { new Guid("3b759e54-64da-41dc-9aca-cde72c844364"), "user2@example.com", "hashedPassword", 341851734, "User2" },
                    { new Guid("47858dba-8e9e-41f3-ac6b-956306181751"), "user8@example.com", "hashedPassword", 1311292502, "User8" },
                    { new Guid("4a5bf110-3a98-4c9c-a7a0-2cb84c363168"), "user16@example.com", "hashedPassword", 1036067702, "User16" },
                    { new Guid("4f2a2546-ff08-40cb-8234-d8b03e64f135"), "user10@example.com", "hashedPassword", 319576108, "User10" },
                    { new Guid("78cb9f22-4bbf-48ad-b133-e673237cfbaf"), "user13@example.com", "hashedPassword", 811244070, "User13" },
                    { new Guid("a3854e05-28fb-4451-b5a8-386e1f520622"), "user14@example.com", "hashedPassword", 1063087737, "User14" },
                    { new Guid("a48d09a8-049f-44fa-8a03-68dbdd8cea1d"), "user11@example.com", "hashedPassword", 2072233842, "User11" },
                    { new Guid("a86e82fa-622f-4ead-a8f3-8a799be20155"), "user17@example.com", "hashedPassword", 1929035529, "User17" },
                    { new Guid("c2053672-96d8-43fb-8641-35436480752e"), "user18@example.com", "hashedPassword", 2137347066, "User18" },
                    { new Guid("cd8431de-7ef3-4bc3-8e40-37a86bff35fb"), "user3@example.com", "hashedPassword", 1431988776, "User3" },
                    { new Guid("d42d0995-fa26-4c38-aa5d-b0f412f72aea"), "user9@example.com", "hashedPassword", 749943798, "User9" },
                    { new Guid("de7b5011-ef2d-49a2-99ae-c0f119fc3044"), "user15@example.com", "hashedPassword", 2103111965, "User15" },
                    { new Guid("ed4b5b0f-ef97-4667-a87c-e0198e187d19"), "user1@example.com", "hashedPassword", 2080427802, "User1" },
                    { new Guid("f3c88bf5-d54e-4aa2-9edf-6eb19d0f2122"), "user7@example.com", "hashedPassword", 1528357293, "User7" },
                    { new Guid("ff5b1fa2-5c4b-4e7a-990f-a8d34f4e6bfe"), "user20@example.com", "hashedPassword", 75333798, "User20" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Latitude", "Longitude", "UserId" },
                values: new object[,]
                {
                    { new Guid("09dd9a99-d999-448a-b4b0-8e7a42ab89af"), 44.74511396416333, 20.497349903979902, new Guid("07954be9-1416-4c31-8aec-4526d785052f") },
                    { new Guid("0ac1c603-b6c7-48e0-b6c4-0d460a41c919"), 44.808743410710903, 20.379587718310489, new Guid("39c4f7a2-6eb4-4716-8b06-c8b9b28fcfe1") },
                    { new Guid("0b496362-1661-46a7-9296-dadd11449751"), 44.739755979736444, 20.36428440098209, new Guid("08b6de12-8c71-494f-8580-bcfa2a3289f5") },
                    { new Guid("1f7f94b1-9086-4cbc-8305-2cca010aac80"), 44.815052209915585, 20.500449542006542, new Guid("ed4b5b0f-ef97-4667-a87c-e0198e187d19") },
                    { new Guid("35755449-236f-48ba-bc7d-f839aa6ac38e"), 44.817896763964299, 20.494372987074879, new Guid("de7b5011-ef2d-49a2-99ae-c0f119fc3044") },
                    { new Guid("3620e695-e67b-41fc-800d-df675a7e399a"), 44.762168396599861, 20.366240887652292, new Guid("ff5b1fa2-5c4b-4e7a-990f-a8d34f4e6bfe") },
                    { new Guid("37aad600-9caf-454b-a39e-a5ac98743d0a"), 44.777261714064373, 20.410337914924423, new Guid("78cb9f22-4bbf-48ad-b133-e673237cfbaf") },
                    { new Guid("405bcf21-192c-4abb-b29f-f5d8e2b4cfa0"), 44.78516158142822, 20.470297186766704, new Guid("a3854e05-28fb-4451-b5a8-386e1f520622") },
                    { new Guid("450e4b9f-a99e-4882-bbb7-46b740ea127f"), 44.743014464918211, 20.519409199056991, new Guid("a48d09a8-049f-44fa-8a03-68dbdd8cea1d") },
                    { new Guid("490b4bb0-74fb-43e4-bcb7-93c87aac84a2"), 44.78993044294284, 20.385673636593243, new Guid("3b759e54-64da-41dc-9aca-cde72c844364") },
                    { new Guid("50191edd-4a5c-441d-a834-432d7f4fc90f"), 44.753968612457079, 20.454024177400711, new Guid("4a5bf110-3a98-4c9c-a7a0-2cb84c363168") },
                    { new Guid("66a7a985-adb0-4eaf-acdc-1e0598e66e2d"), 44.810617801280273, 20.542356854502593, new Guid("3632ed47-ba65-46d0-b3ba-ec50788933ff") },
                    { new Guid("6d7b93ec-bede-43d8-bb91-7f59a5bab1a0"), 44.835827333430387, 20.460244978351422, new Guid("d42d0995-fa26-4c38-aa5d-b0f412f72aea") },
                    { new Guid("743cf9bc-9f0a-48e3-a6e2-39736e7b73a7"), 44.754111100183074, 20.45266152470095, new Guid("4f2a2546-ff08-40cb-8234-d8b03e64f135") },
                    { new Guid("8b11b8ba-9b73-4182-a8a0-af196d5be345"), 44.789821451875774, 20.367093239820761, new Guid("cd8431de-7ef3-4bc3-8e40-37a86bff35fb") },
                    { new Guid("b0d7bcb5-d4be-4ce2-b4e4-b6fe24aa6869"), 44.788264859529512, 20.367158546633799, new Guid("a86e82fa-622f-4ead-a8f3-8a799be20155") },
                    { new Guid("b83d8ec7-9020-4969-8ed4-3c3635d86521"), 44.835509940101872, 20.507731048789907, new Guid("c2053672-96d8-43fb-8641-35436480752e") },
                    { new Guid("cb9d8357-7426-4fb1-a511-ffe8ea619b87"), 44.771040026023819, 20.488791104084296, new Guid("47858dba-8e9e-41f3-ac6b-956306181751") },
                    { new Guid("cbccd97f-b27c-4135-b654-1ceed33da688"), 44.77991543923585, 20.452452771930304, new Guid("1de0ff64-d9d0-4cc4-b53d-7b1231fcdcce") },
                    { new Guid("cce5ffca-042e-455d-96e0-77bebe5aa8a8"), 44.820820150827153, 20.484971798441034, new Guid("f3c88bf5-d54e-4aa2-9edf-6eb19d0f2122") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_UserId",
                table: "Locations",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
