using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MachineManagementSystemVer2.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
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
                    CompanyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    PlantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PlantCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
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
                name: "SystemLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RecordId = table.Column<int>(type: "int", nullable: true),
                    ActionTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_SystemLogs_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
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
                    ProductionLine = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    CustomerContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    PersonId1 = table.Column<int>(type: "int", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepairCases_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepairCases_Persons_PersonId1",
                        column: x => x.PersonId1,
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "CaseComments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseComments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_CaseComments_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
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
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    OldStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseHistories", x => x.HistoryId);
                    table.ForeignKey(
                        name: "FK_CaseHistories_Persons_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CaseHistories_RepairCases_CaseId",
                        column: x => x.CaseId,
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
                columns: new[] { "CustomerId", "Address", "CompanyName", "Phone", "TaxId" },
                values: new object[,]
                {
                    { 1, "台南市新市區", "T公司", "06-111-2222", "12345678" },
                    { 2, "台北市", "發哥", "06-111-2233", "11223344" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "Account", "Address", "EmergencyContact", "EmergencyPhone", "HireDate", "Name", "PasswordHash", "Phone", "Remark", "Role", "Title" },
                values: new object[,]
                {
                    { 1, null, "屏東市xxxxxxxxx", "王媽媽", "0987654321", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "王小明", null, "0912345678", null, null, "現場工程師" },
                    { 2, null, "新竹市xxxxxx", "李妻", "03-1234567", new DateTime(2012, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "李主管", null, "0952368741", null, null, "業務經理" },
                    { 3, null, "高雄市xxxxxxxxx", "陳媽媽", "0987654321", new DateTime(2022, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "陳大華", null, "0919874585", null, null, "現場工程師" },
                    { 4, null, "苗栗市xxxxxxxxx", "張嬸", "0987612587", new DateTime(2020, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "張經理", null, "0987258678", null, null, "工程部經理" },
                    { 5, null, "高雄市xxxxxxxxx", "工程師", "0987612587", new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "系統管理員", null, "0987888888", null, null, "系統管理員" }
                });

            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "PlantId", "Address", "CustomerId", "Phone", "PlantCode", "PlantName" },
                values: new object[,]
                {
                    { 1, "新竹市xoxox", 1, "03-1224545", "H1", "新竹廠" },
                    { 2, "台南市xoxox", 1, "06-6548452", "N1", "台南廠" },
                    { 3, "新竹縣xoxox", 2, "03-2116555", "RC1", "竹科研發中心" }
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "DeviceId", "Model", "PlantId", "ProductionLine", "Remark", "SerialNumber" },
                values: new object[,]
                {
                    { 1, "NXE3400", 1, "H1#1", null, "20240901-001" },
                    { 2, "NXE3400", 2, "N1#1", null, "20240901-002" },
                    { 3, "ASM9000", 3, "RC1#1", null, "20240901-010" }
                });

            migrationBuilder.InsertData(
                table: "RepairCases",
                columns: new[] { "RepairCaseId", "CustomerContact", "CustomerId", "Description", "DeviceId", "Notes", "OccurredAt", "PersonId", "PersonId1", "Status" },
                values: new object[,]
                {
                    { 1, null, null, "客戶反應光刻機曝光後晶圓良率降低，需要檢查光源模組", 1, null, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "OPEN" },
                    { 2, null, null, "蝕刻腔體真空無法維持，懷疑是真空幫浦老化", 2, null, new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "暫置" },
                    { 3, null, null, "自動測試程式頻繁跳出錯誤代碼，需要檢查控制模組", 3, null, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null, "CLOSE" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_CaseId",
                table: "CaseComments",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseComments_PersonId",
                table: "CaseComments",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_CaseId",
                table: "CaseHistories",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseHistories_ChangedBy",
                table: "CaseHistories",
                column: "ChangedBy");

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
                name: "IX_RepairCases_PersonId",
                table: "RepairCases",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairCases_PersonId1",
                table: "RepairCases",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_PersonId",
                table: "SystemLogs",
                column: "PersonId");
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
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "RepairCases");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
