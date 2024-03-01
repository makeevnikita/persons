﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skills.Migrations
{
    /// <inheritdoc />
    public partial class SetDefaultSkillsValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Person",
                type: "text",
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Skills",
                table: "Person",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValue: "[]");
        }
    }
}
