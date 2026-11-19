using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYA_PortfolioProject.Migrations;

/// <inheritdoc />
public partial class _20261117201020_mig1 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Abouts",
            columns: table => new
            {
                AboutId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                MainImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                NameSurname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Profession = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                AboutCVUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Abouts", x => x.AboutId);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Abouts");
    }
}
