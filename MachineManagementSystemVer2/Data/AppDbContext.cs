using MachineManagementSystemVer2.Models;
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<RepairCase> RepairCases { get; set; }
        public DbSet<CaseComment> CaseComments { get; set; }
        public DbSet<CasePhoto> CasePhotos { get; set; }
        public DbSet<CaseHistory> CaseHistories { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Customers 客戶
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    CompanyName = "T公司",
                    TaxId = "12345678",
                    Address = "台南市新市區",
                    Phone = "06-111-2222"
                },
                new Customer
                {
                    CustomerId = 2,
                    CompanyName = "發哥",
                    TaxId = "11223344",
                    Address = "台北市",
                    Phone = "06-111-2233"
                }
            );

            // Plants 廠區
            modelBuilder.Entity<Plant>().HasData(
                new Plant
                {
                    PlantId = 1,
                    PlantName = "新竹廠",
                    PlantCode = "H1",
                    Address = "新竹市xoxox",
                    Phone = "03-1224545",
                    CustomerId = 1
                },
                new Plant
                {
                    PlantId = 2,
                    PlantName = "台南廠",
                    PlantCode = "N1",
                    Address = "台南市xoxox",
                    Phone = "06-6548452",
                    CustomerId = 1
                },
                new Plant
                {
                    PlantId = 3,
                    PlantName = "竹科研發中心",
                    PlantCode = "RC1",
                    Address = "新竹縣xoxox",
                    Phone = "03-2116555",
                    CustomerId = 2
                }
            );

            // Devices 設備
            modelBuilder.Entity<Device>().HasData(
                new Device
                {
                    DeviceId = 1,
                    SerialNumber = "20240901-001",
                    Model = "NXE3400",
                    ProductionLine = "H1#1",
                    PlantId = 1,
                },
                new Device
                {
                    DeviceId = 2,
                    SerialNumber = "20240901-002",
                    Model = "NXE3400",
                    ProductionLine = "N1#1",
                    PlantId = 2,
                },
                new Device
                {
                    DeviceId = 3,
                    SerialNumber = "20240901-010",
                    Model = "ASM9000",
                    ProductionLine = "RC1#1",
                    PlantId = 3,
                }
            );

            // Persons 人員
            modelBuilder.Entity<Person>().HasData(
                new Person
                {
                    PersonId = 1,
                    Name = "王小明",
                    HireDate = new DateTime(2022, 01, 01),
                    Title = "現場工程師",
                    Address = "屏東市xxxxxxxxx",
                    Phone = "0912345678",
                    EmergencyContact = "王媽媽",
                    EmergencyPhone = "0987654321",
                },
                new Person
                {
                    PersonId = 2,
                    Name = "李主管",
                    HireDate = new DateTime(2012, 01, 01),
                    Title = "業務經理",
                    Address = "新竹市xxxxxx",
                    Phone = "0952368741",
                    EmergencyContact = "李妻",
                    EmergencyPhone = "03-1234567",
                },
                new Person
                {
                    PersonId = 3,
                    Name = "陳大華",
                    HireDate = new DateTime(2022, 06, 01),
                    Title = "現場工程師",
                    Address = "高雄市xxxxxxxxx",
                    Phone = "0919874585",
                    EmergencyContact = "陳媽媽",
                    EmergencyPhone = "0987654321",
                },
                new Person
                {
                    PersonId = 4,
                    Name = "張經理",
                    HireDate = new DateTime(2020, 09, 08),
                    Title = "工程部經理",
                    Address = "苗栗市xxxxxxxxx",
                    Phone = "0987258678",
                    EmergencyContact = "張嬸",
                    EmergencyPhone = "0987612587",
                },
                new Person
                {
                    PersonId = 5,
                    Name = "系統管理員",
                    HireDate = new DateTime(2025, 09, 05),
                    Title = "系統管理員",
                    Address = "高雄市xxxxxxxxx",
                    Phone = "0987888888",
                    EmergencyContact = "工程師",
                    EmergencyPhone = "0987612587",
                }
            );

            // RepairCases 維修案件
            modelBuilder.Entity<RepairCase>().HasData(
                new RepairCase
                {
                    RepairCaseId = 1,
                    Description = "客戶反應光刻機曝光後晶圓良率降低，需要檢查光源模組",
                    Status = "OPEN",
                    OccurredAt = new DateTime(2024, 09, 01),
                    DeviceId = 1,
                    PersonId = 1,
                },
                new RepairCase
                {
                    RepairCaseId = 2,
                    Description = "蝕刻腔體真空無法維持，懷疑是真空幫浦老化",
                    Status = "暫置",
                    OccurredAt = new DateTime(2024, 09, 02),
                    DeviceId = 2,
                    PersonId = 2,
                },
                new RepairCase
                {
                    RepairCaseId = 3,
                    Description = "自動測試程式頻繁跳出錯誤代碼，需要檢查控制模組",
                    Status = "CLOSE",
                    OccurredAt = new DateTime(2024, 09, 03),
                    DeviceId = 3,
                    PersonId = 3,
                }

            );
            // ✅ RepairCase 與 CaseComment → Cascade Delete
            modelBuilder.Entity<CaseComment>()
                .HasOne(c => c.RepairCase)
                .WithMany(r => r.Comments)
                .HasForeignKey(c => c.CaseId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 Device -> RepairCase 不要 Cascade
            modelBuilder.Entity<RepairCase>()
                .HasOne(rc => rc.Device)
                .WithMany(d => d.RepairCases)
                .HasForeignKey(rc => rc.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔹 Person -> RepairCase 不要 Cascade
            modelBuilder.Entity<RepairCase>()
                .HasOne(rc => rc.Person)
                .WithMany()
                .HasForeignKey(rc => rc.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
    


