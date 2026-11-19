using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MYA_PortfolioProject.Migrations;

/// <inheritdoc />
public partial class _20261118135042_mig6 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Certificates",
            columns: table => new
            {
                CertificateId = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Institution = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Date = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CertificateUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Certificates", x => x.CertificateId);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Certificates");
    }
}
