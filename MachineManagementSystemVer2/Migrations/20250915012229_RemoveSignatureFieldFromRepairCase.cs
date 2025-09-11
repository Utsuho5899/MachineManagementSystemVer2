using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineManagementSystemVer2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSignatureFieldFromRepairCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "RepairCases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "RepairCases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 1,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 2,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 3,
                columns: new[] { "EndTime", "StartTime" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RepairCases");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RepairCases");
        }
    }
}
