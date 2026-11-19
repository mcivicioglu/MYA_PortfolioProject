using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYA_PortfolioProject.Migrations;

/// <inheritdoc />
public partial class _20261118092504_mig4 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "HeroTitles",
            columns: table => new
            {
                HeroTitleId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Order = table.Column<int>(type: "int", nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HeroTitles", x => x.HeroTitleId);
            });

        migrationBuilder.CreateTable(
            name: "Messages",
            columns: table => new
            {
                MessageId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SenderName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SenderEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                SenderSubject = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsRead = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Messages", x => x.MessageId);
            });

        migrationBuilder.CreateTable(
            name: "Projects",
            columns: table => new
            {
                ProjectId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProjectTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                Technology = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Projects", x => x.ProjectId);
                table.ForeignKey(
                    name: "FK_Projects_Categories_CategoryId",
                    column: x => x.CategoryId,
                    principalTable: "Categories",
                    principalColumn: "CategoryId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Skills",
            columns: table => new
            {
                SkillId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                IconURL = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                SkillValue = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Skills", x => x.SkillId);
            });

        migrationBuilder.CreateTable(
            name: "SocialLinks",
            columns: table => new
            {
                SocialLinkId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Platform = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SocialLinks", x => x.SocialLinkId);
            });

        migrationBuilder.CreateTable(
            name: "SkillItems",
            columns: table => new
            {
                SkillItemId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SkillItemTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                SkillId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SkillItems", x => x.SkillItemId);
                table.ForeignKey(
                    name: "FK_SkillItems_Skills_SkillId",
                    column: x => x.SkillId,
                    principalTable: "Skills",
                    principalColumn: "SkillId",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Projects_CategoryId",
            table: "Projects",
            column: "CategoryId");

        migrationBuilder.CreateIndex(
            name: "IX_SkillItems_SkillId",
            table: "SkillItems",
            column: "SkillId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "HeroTitles");

        migrationBuilder.DropTable(
            name: "Messages");

        migrationBuilder.DropTable(
            name: "Projects");

        migrationBuilder.DropTable(
            name: "SkillItems");

        migrationBuilder.DropTable(
            name: "SocialLinks");

        migrationBuilder.DropTable(
            name: "Skills");
    }
}
