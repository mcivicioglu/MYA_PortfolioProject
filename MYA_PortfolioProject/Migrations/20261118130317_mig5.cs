using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYA_PortfolioProject.Migrations;

/// <inheritdoc />
public partial class _20261118130317_mig5 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "IconUrl",
            table: "SkillItems",
            type: "nvarchar(150)",
            maxLength: 150,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "IconUrl",
            table: "SkillItems");
    }
}
