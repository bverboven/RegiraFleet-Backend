using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Regira.Fleet.Data.SqlServer.Migrations;

/// <inheritdoc />
public partial class InitialStatisticsViews : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var sql in StatisticsViews.All)
        {
            migrationBuilder.Sql(sql);
        }
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
