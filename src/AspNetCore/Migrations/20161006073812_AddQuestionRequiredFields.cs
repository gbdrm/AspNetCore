using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCore.Migrations
{
    public partial class AddQuestionRequiredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "TestItems",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "TestItems",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "TestItems",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "TestItems",
                nullable: true);
        }
    }
}
