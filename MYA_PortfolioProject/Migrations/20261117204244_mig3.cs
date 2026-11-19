using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYA_PortfolioProject.Migrations;

/// <inheritdoc />
public partial class _20261117204244_mig3 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                CategoryId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categories", x => x.CategoryId);
            });

        migrationBuilder.CreateTable(
            name: "Contacts",
            columns: table => new
            {
                ContactId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contacts", x => x.ContactId);
            });

        migrationBuilder.CreateTable(
            name: "Educations",
            columns: table => new
            {
                EducationId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                SchoolName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                StartYear = table.Column<int>(type: "int", nullable: false),
                EndYear = table.Column<int>(type: "int", nullable: true),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Educations", x => x.EducationId);
            });

        migrationBuilder.CreateTable(
            name: "Experiences",
            columns: table => new
            {
                ExperienceId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                IsActive = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Experiences", x => x.ExperienceId);
            });

        migrationBuilder.CreateTable(
            name: "HeroSections",
            columns: table => new
            {
                HeroSectionId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Greeting = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                ImageURL = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HeroSections", x => x.HeroSectionId);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Categories");

        migrationBuilder.DropTable(
            name: "Contacts");

        migrationBuilder.DropTable(
            name: "Educations");

        migrationBuilder.DropTable(
            name: "Experiences");

        migrationBuilder.DropTable(
            name: "HeroSections");
    }
}
