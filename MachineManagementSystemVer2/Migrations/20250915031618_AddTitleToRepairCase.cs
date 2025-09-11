using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineManagementSystemVer2.Migrations
{
    /// <inheritdoc />
    public partial class AddTitleToRepairCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "RepairCases",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 1,
                column: "Title",
                value: "光刻機光源模組檢查");

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 2,
                column: "Title",
                value: "蝕刻腔體真空度異常");

            migrationBuilder.UpdateData(
                table: "RepairCases",
                keyColumn: "RepairCaseId",
                keyValue: 3,
                column: "Title",
                value: "自動測試程式錯誤");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "RepairCases");
        }
    }
}
