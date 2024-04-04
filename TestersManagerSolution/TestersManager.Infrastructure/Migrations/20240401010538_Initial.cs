using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TestersManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TesterName = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DevStreams",
                columns: table => new
                {
                    DevStreamId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DevStreamName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevStreams", x => x.DevStreamId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
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
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "Testers",
                columns: table => new
                {
                    TesterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TesterName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Gender = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DevStreamId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Position = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    WorksFor = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 1),
                    Skills = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testers", x => x.TesterId);
                    table.ForeignKey(
                        name: "FK_Testers_DevStreams_DevStreamId",
                        column: x => x.DevStreamId,
                        principalTable: "DevStreams",
                        principalColumn: "DevStreamId");
                });

            migrationBuilder.InsertData(
                table: "DevStreams",
                columns: new[] { "DevStreamId", "DevStreamName" },
                values: new object[,]
                {
                    { new Guid("02df3b54-16f9-44c7-9272-c57873f8a2ca"), "Tech" },
                    { new Guid("1a76b36b-4b06-4a69-a368-7ade27ab739e"), "New Year" },
                    { new Guid("248a6fe4-ac09-452c-a205-a6cc4b7e9e56"), "Crew" },
                    { new Guid("78fd1d57-28e2-4cd8-82a3-5dfdba2a212a"), "Artillery" },
                    { new Guid("97be8c70-e9aa-41d8-9bc6-f8832c1b485a"), "Core" }
                });

            migrationBuilder.InsertData(
                table: "Testers",
                columns: new[] { "TesterId", "BirthDate", "DevStreamId", "Email", "Gender", "WorksFor", "Position", "Skills", "TesterName" },
                values: new object[,]
                {
                    { new Guid("286e7c8d-759e-445a-9700-c82c15ee72c5"), new DateTime(1999, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("02df3b54-16f9-44c7-9272-c57873f8a2ca"), "bdanev3@posterous.com", "Female", 60, "Senior QA", "Frs", "Marie Danev" },
                    { new Guid("3c19db1f-cdf0-486f-8d85-18a481c29d76"), new DateTime(1991, 9, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("78fd1d57-28e2-4cd8-82a3-5dfdba2a212a"), "jdetheridge9@msn.com", "Male", 9, "Middle QA", "JavaScript", "Jason Detheridge" },
                    { new Guid("3e2b5484-3a41-4f40-8126-babbfb4b4cd2"), new DateTime(1999, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1a76b36b-4b06-4a69-a368-7ade27ab739e"), "apadbery6@cloudflare.com", "Male", 20, "Junior QA", "Python", "Alexander Padbery" },
                    { new Guid("518f9fea-bf73-497d-a76f-eec40204dafa"), new DateTime(1995, 6, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("78fd1d57-28e2-4cd8-82a3-5dfdba2a212a"), "easch5@upenn.edu", "Female", 52, "G-ops", "JavaScript", "Eleonore Asch" },
                    { new Guid("6ff4bbba-55e4-48b9-aed6-ad352d082e05"), new DateTime(1998, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("97be8c70-e9aa-41d8-9bc6-f8832c1b485a"), "afruish2@multiply.com", "Male", 36, "G-ops", "Frs", "Alex Fruish" },
                    { new Guid("73452c7a-4206-499c-98fa-277407c2c23d"), new DateTime(1989, 9, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1a76b36b-4b06-4a69-a368-7ade27ab739e"), "tmccard1@webster.com", "Female", 30, "Senior QA", "Python", "Tarrah McCard" },
                    { new Guid("957b658d-ed53-484b-9f9a-6d741657decd"), new DateTime(1989, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("1a76b36b-4b06-4a69-a368-7ade27ab739e"), "amaty4@scribd.com", "Male", 9, "Middle QA", "CW", "Arman Maty" },
                    { new Guid("9b379a26-9220-4bf1-bb70-193e2ada3313"), new DateTime(1986, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("97be8c70-e9aa-41d8-9bc6-f8832c1b485a"), "sadame7@npr.org", "Female", 14, "Junior QA", "Python", "Shana Adame" },
                    { new Guid("c3250d4a-cd91-4b73-a535-ac0e5bede0fa"), new DateTime(1989, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("02df3b54-16f9-44c7-9272-c57873f8a2ca"), "cmumford8@histats.com", "Female", 1, "Intern", "Blitz", "Cherish Mumford" },
                    { new Guid("e83987f1-e884-446c-901f-978fc909babf"), new DateTime(1994, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("248a6fe4-ac09-452c-a205-a6cc4b7e9e56"), "rlightman0@uol.com.br", "Female", 3, "Middle QA", "JavaScript", "Tanya Lightman" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Testers_DevStreamId",
                table: "Testers",
                column: "DevStreamId");
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
                name: "Testers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DevStreams");
        }
    }
}
