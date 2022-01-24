using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeDetails",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RateFormated = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    DiffFormated = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    Rate = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Diff = table.Column<decimal>(type: "DECIMAL(18,5)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    ValidFromDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeDetails", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeDetails");
        }
    }
}
