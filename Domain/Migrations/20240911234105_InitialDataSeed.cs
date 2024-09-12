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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    { new Guid("01edcb56-91d2-4cc8-9d01-77c65dbcfe8a"), "user12@example.com", "hashedPassword", 1156976611, "User12" },
                    { new Guid("05dfe9a3-74ee-4690-ac0d-5a47790dacb1"), "user9@example.com", "hashedPassword", 749943798, "User9" },
                    { new Guid("1b19ced6-b101-4f07-b016-fa02d4a42045"), "user6@example.com", "hashedPassword", 2037243568, "User6" },
                    { new Guid("25b387db-7278-4053-882a-460a279cfb89"), "user11@example.com", "hashedPassword", 2072233842, "User11" },
                    { new Guid("43d8c1c3-1340-4d66-95bb-b17593524b5f"), "user14@example.com", "hashedPassword", 1063087737, "User14" },
                    { new Guid("4eac8fd1-4d66-4406-86a2-617cdbe08b73"), "user5@example.com", "hashedPassword", 761513014, "User5" },
                    { new Guid("4fae1fe1-f12c-4f63-81a7-9ab5972dbf4b"), "user17@example.com", "hashedPassword", 1929035529, "User17" },
                    { new Guid("64766971-8bba-4681-a70c-fd09e1ddbe65"), "user16@example.com", "hashedPassword", 1036067702, "User16" },
                    { new Guid("6c6db130-2568-4da5-88cb-52ad59cfaf24"), "user1@example.com", "hashedPassword", 2080427802, "User1" },
                    { new Guid("7592f215-8502-4bb0-9098-2b96a0acf651"), "user15@example.com", "hashedPassword", 2103111965, "User15" },
                    { new Guid("800db62f-3ebf-4c7f-86b5-5de52bd60e2e"), "user7@example.com", "hashedPassword", 1528357293, "User7" },
                    { new Guid("cda85adb-13b3-49e5-b4f8-f559d758af13"), "user20@example.com", "hashedPassword", 75333798, "User20" },
                    { new Guid("d0f03af2-99db-48b3-8295-9dce0dcb4cfe"), "user3@example.com", "hashedPassword", 1431988776, "User3" },
                    { new Guid("d2d2331f-1833-474a-a83f-aa43fecbc774"), "user18@example.com", "hashedPassword", 2137347066, "User18" },
                    { new Guid("d582205b-c469-46d9-a7aa-ae576072b924"), "user19@example.com", "hashedPassword", 825105703, "User19" },
                    { new Guid("dbc86c9d-de55-4335-9d7c-eb5a4d4bbab4"), "user10@example.com", "hashedPassword", 319576108, "User10" },
                    { new Guid("df1dcc6e-7fef-4a78-915d-d1477478885c"), "user13@example.com", "hashedPassword", 811244070, "User13" },
                    { new Guid("e5b517e2-cec4-4573-9dd6-66d665975a29"), "user2@example.com", "hashedPassword", 341851734, "User2" },
                    { new Guid("f336fa9d-d49e-44ac-b4e6-6a09e2563ffa"), "user4@example.com", "hashedPassword", 1938005744, "User4" },
                    { new Guid("f9e22a9d-63c9-43e8-b765-cbd5b56a76ee"), "user8@example.com", "hashedPassword", 1311292502, "User8" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "Latitude", "Longitude", "UserId" },
                values: new object[,]
                {
                    { new Guid("0ccd995c-0f19-44b0-a576-89bdbed0f0a7"), 44.775315751918335, 20.382189476462727, new Guid("01edcb56-91d2-4cc8-9d01-77c65dbcfe8a") },
                    { new Guid("23c0a21d-a683-45d4-9be9-9505103a777b"), 44.78444587104417, 20.506561997595561, new Guid("6c6db130-2568-4da5-88cb-52ad59cfaf24") },
                    { new Guid("25e3e6cb-862e-4f6d-9304-ff7c44640876"), 44.756345702820468, 20.364252110647563, new Guid("43d8c1c3-1340-4d66-95bb-b17593524b5f") },
                    { new Guid("360b760f-3a13-44f5-be23-0b97641951f2"), 44.79368531746654, 20.376486430682306, new Guid("800db62f-3ebf-4c7f-86b5-5de52bd60e2e") },
                    { new Guid("4ad92814-a4a7-4627-ba6a-b1bae33ae75e"), 44.75021714887216, 20.365379724076913, new Guid("d0f03af2-99db-48b3-8295-9dce0dcb4cfe") },
                    { new Guid("51e4cf40-96ec-4de0-b21a-131099dc6e52"), 44.770011237020277, 20.545955878670078, new Guid("df1dcc6e-7fef-4a78-915d-d1477478885c") },
                    { new Guid("5cca7453-dcbc-4624-a4d2-1fa3c90b55dd"), 44.743172768617661, 20.468367697141574, new Guid("4fae1fe1-f12c-4f63-81a7-9ab5972dbf4b") },
                    { new Guid("5d2dfd64-c5a6-4428-a678-439e9d7f377e"), 44.768465745843308, 20.537194751916221, new Guid("d2d2331f-1833-474a-a83f-aa43fecbc774") },
                    { new Guid("6dc1428a-819e-45b5-8d49-c48c871dd071"), 44.737240335169169, 20.383850853276368, new Guid("05dfe9a3-74ee-4690-ac0d-5a47790dacb1") },
                    { new Guid("7c1c9bfd-a4cf-4e02-9fda-5b4d5348b9fc"), 44.769137780494837, 20.487320611314438, new Guid("d582205b-c469-46d9-a7aa-ae576072b924") },
                    { new Guid("7f88bc82-33cd-466b-995c-9802e45a3bb2"), 44.823107921135147, 20.365084575526154, new Guid("25b387db-7278-4053-882a-460a279cfb89") },
                    { new Guid("8016c5d0-4c85-4d3e-95f0-0725a99c8a97"), 44.753557166940759, 20.36085886165025, new Guid("4eac8fd1-4d66-4406-86a2-617cdbe08b73") },
                    { new Guid("85213472-5370-4d02-823b-c10ec69fcdbd"), 44.771322161189005, 20.498980019133477, new Guid("7592f215-8502-4bb0-9098-2b96a0acf651") },
                    { new Guid("87dea081-a3d8-4e6c-aa0b-d53dbf479c54"), 44.752930576823481, 20.499037205848172, new Guid("64766971-8bba-4681-a70c-fd09e1ddbe65") },
                    { new Guid("8bc4cd4e-9dcf-4ea3-970f-924365233ddd"), 44.747220636479334, 20.544206815861532, new Guid("dbc86c9d-de55-4335-9d7c-eb5a4d4bbab4") },
                    { new Guid("8caa5e1c-ce9d-4c34-b1a9-882bea49f3e9"), 44.797505877434951, 20.521308321551484, new Guid("cda85adb-13b3-49e5-b4f8-f559d758af13") },
                    { new Guid("b5ddcb10-ab00-4798-ae73-698412ca9f02"), 44.777684928395495, 20.506324566143647, new Guid("f336fa9d-d49e-44ac-b4e6-6a09e2563ffa") },
                    { new Guid("be9af966-cbd3-4797-b491-2717e6b42586"), 44.755114648822023, 20.538514183711797, new Guid("1b19ced6-b101-4f07-b016-fa02d4a42045") },
                    { new Guid("d376ad1d-6dd5-41ce-ab5b-9377aad2ee96"), 44.745673663194601, 20.551324516628437, new Guid("f9e22a9d-63c9-43e8-b765-cbd5b56a76ee") },
                    { new Guid("edfa095a-71b1-4738-ac84-6edaaa58645c"), 44.784772702099389, 20.403211196282172, new Guid("e5b517e2-cec4-4573-9dd6-66d665975a29") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
