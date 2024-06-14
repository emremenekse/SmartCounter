using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CounterService.Migrations
{
    /// <inheritdoc />
    public partial class mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterReadings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    MeasurementTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastIndex = table.Column<decimal>(type: "numeric", nullable: false),
                    Voltage = table.Column<decimal>(type: "numeric", nullable: false),
                    Current = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReadings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterReadings");
        }
    }
}
