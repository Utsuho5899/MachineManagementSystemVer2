using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MachineManagementSystemVer2.Migrations
{
    /// <inheritdoc />
    public partial class FinalModelSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerTaxId = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeTitle = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    EmployeeAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlantCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    PlantAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PlantPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.PlantId);
                    table.ForeignKey(
                        name: "FK_Plants_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    ProductionLine = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepairCases",
                columns: table => new
                {
                    RepairCaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CustomerContact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    CaseRemark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairCases", x => x.RepairCaseId);
                    table.ForeignKey(
                        name: "FK_RepairCases_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK_RepairCases_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepairCases_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepairCases_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "PlantId");
                });

            migrationBuilder.CreateTable(
                name: "CaseComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    CaseComments = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CaseComments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseComments_RepairCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "RepairCases",
                        principalColumn: "RepairCaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaseHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairCaseId = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_CaseHistories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseHistories_RepairCases_RepairCaseId",
                        column: x => x.RepairCaseId,
                        principalTable: "RepairCases",
                        principalColumn: "RepairCaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePhotos",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    PhotoData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePhotos", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_CasePhotos_RepairCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "RepairCases",
                        principalColumn: "RepairCaseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CustomerAddress", "CustomerName", "CustomerPhone", "CustomerTaxId" },
                values: new object[,]
                {
                    { 1, "台南市新市區", "T公司", "06-111-2222", "12345678" },
                    { 2, "台北市", "發哥", "06-111-2233", "11223344" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Account", "EmergencyContact", "EmergencyPhone", "EmployeeAddress", "EmployeeName", "EmployeePhone", "EmployeeTitle", "HireDate", "Password", "Remarks" },
                values: new object[,]
                {
                    { 1, null, "王媽媽", "0987654321", "屏東市xxxxxxxxx", "王小明", "0912345678", "現場工程師", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 2, null, "李妻", "03-1234567", "新竹市xxxxxx", "李主管", "0952368741", "業務經理", new DateTime(2012, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 3, null, "陳媽媽", "0987654321", "高雄市xxxxxxxxx", "陳大華", "0919874585", "現場工程師", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 4, null, "張嬸", "0987612587", "苗栗市xxxxxxxxx", "張經理", "0987258678", "工程部經理", new DateTime(2020, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 5, null, "工程師", "0987612587", "高雄市xxxxxxxxx", "系統管理員", "0987888888", "系統管理員", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "PlantId", "CustomerId", "PlantAddress", "PlantCode", "PlantName", "PlantPhone" },
                values: new object[,]
                {
                    { 1, 1, "新竹市xoxox", "H1", "新竹廠", "03-1224545" },
                    { 2, 1, "台南市xoxox", "N1", "台南廠", "06-6548452" },
                    { 3, 2, "新竹縣xoxox", "RC1", "竹科研發中心", "03-2116555" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "DeviceModel", "PlantId", "ProductionLine", "Remark", "SerialNumber" },
                values: new object[,]
                {
                    { 1, "NXE3400", 1, "H1#1", null, "20240901-001" },
                    { 2, "NXE3400", 2, "N1#1", null, "20240901-002" },
                    { 3, "ASM9000", 3, "RC1#1", null, "20240901-010" }
                });

            migrationBuilder.InsertData(
                table: "RepairCases",
                columns: new[] { "RepairCaseId", "CaseRemark", "CaseStatus", "CustomerContact", "CustomerId", "Description", "DeviceId", "EmployeeId", "OccurredAt", "PlantId" },
                values: new object[,]
                {
                    { 1, null, "OPEN", null, null, "客戶反應光刻機曝光後晶圓良率降低，需要檢查光源模組", 1, 1, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, null, "暫置", null, null, "蝕刻腔體真空無法維持，懷疑是真空幫浦老化", 2, 2, new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, null, "CLOSE", null, null, "自動測試程式頻繁跳出錯誤代碼，需要檢查控制模組", 3, 3, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_CaseId",
                table: "CaseComments",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_EmployeeId",
                table: "CaseComments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_EmployeeId",
                table: "CaseHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_RepairCaseId",
                table: "CaseHistories",
                column: "RepairCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePhotos_CaseId",
                table: "CasePhotos",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_PlantId",
                table: "Devices",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_CustomerId",
                table: "Plants",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairCases_CustomerId",
                table: "RepairCases",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairCases_DeviceId",
                table: "RepairCases",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairCases_EmployeeId",
                table: "RepairCases",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairCases_PlantId",
                table: "RepairCases",
                column: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseComments");

            migrationBuilder.DropTable(
                name: "CaseHistories");

            migrationBuilder.DropTable(
                name: "CasePhotos");

            migrationBuilder.DropTable(
                name: "RepairCases");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
