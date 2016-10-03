using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCore.Data.Migrations
{
    public partial class AddBasicTestInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestPackages",
                columns: table => new
                {
                    TestPackageId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TimeCreated = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true),
                    Viewed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPackages", x => x.TestPackageId);
                    table.ForeignKey(
                        name: "FK_TestPackages_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestItems",
                columns: table => new
                {
                    TestItemId = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(nullable: true),
                    Question = table.Column<string>(nullable: true),
                    TestPackageId = table.Column<Guid>(nullable: false),
                    TestType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestItems", x => x.TestItemId);
                    table.ForeignKey(
                        name: "FK_TestItems_TestPackages_TestPackageId",
                        column: x => x.TestPackageId,
                        principalTable: "TestPackages",
                        principalColumn: "TestPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestResults",
                columns: table => new
                {
                    TestResultId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    TestPackageId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    UserId1 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestResults", x => x.TestResultId);
                    table.ForeignKey(
                        name: "FK_TestResults_TestPackages_TestPackageId",
                        column: x => x.TestPackageId,
                        principalTable: "TestPackages",
                        principalColumn: "TestPackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestResults_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TestOptions",
                columns: table => new
                {
                    TestOptionId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    TestItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestOptions", x => x.TestOptionId);
                    table.ForeignKey(
                        name: "FK_TestOptions_TestItems_TestItemId",
                        column: x => x.TestItemId,
                        principalTable: "TestItems",
                        principalColumn: "TestItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestItems_TestPackageId",
                table: "TestItems",
                column: "TestPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TestOptions_TestItemId",
                table: "TestOptions",
                column: "TestItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TestPackages_UserId1",
                table: "TestPackages",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_TestPackageId",
                table: "TestResults",
                column: "TestPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_UserId1",
                table: "TestResults",
                column: "UserId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestOptions");

            migrationBuilder.DropTable(
                name: "TestResults");

            migrationBuilder.DropTable(
                name: "TestItems");

            migrationBuilder.DropTable(
                name: "TestPackages");
        }
    }
}
