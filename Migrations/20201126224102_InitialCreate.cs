using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bugzilla.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Developers",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
                    RoleId = table.Column<byte[]>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Developers_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bugs",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
                    DevId = table.Column<byte[]>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Closed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bugs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bugs_Developers_DevId",
                        column: x => x.DevId,
                        principalTable: "Developers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Fixes",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
                    BugId = table.Column<byte[]>(nullable: true),
                    DevId = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fixes_Bugs_BugId",
                        column: x => x.BugId,
                        principalTable: "Bugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fixes_Developers_DevId",
                        column: x => x.DevId,
                        principalTable: "Developers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<byte[]>(nullable: false),
                    FixId = table.Column<byte[]>(nullable: true),
                    DevId = table.Column<byte[]>(nullable: true),
                    Approved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Developers_DevId",
                        column: x => x.DevId,
                        principalTable: "Developers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Fixes_FixId",
                        column: x => x.FixId,
                        principalTable: "Fixes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bugs_DevId",
                table: "Bugs",
                column: "DevId");

            migrationBuilder.CreateIndex(
                name: "IX_Developers_RoleId",
                table: "Developers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixes_BugId",
                table: "Fixes",
                column: "BugId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixes_DevId",
                table: "Fixes",
                column: "DevId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DevId",
                table: "Reviews",
                column: "DevId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FixId",
                table: "Reviews",
                column: "FixId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Fixes");

            migrationBuilder.DropTable(
                name: "Bugs");

            migrationBuilder.DropTable(
                name: "Developers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
