using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MachineManagementSystemVer2.Data
{
    public class AppDbContext : IdentityDbContext<Employee>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<RepairCase> RepairCases { get; set; }
        public DbSet<CaseComment> CaseComments { get; set; }
        public DbSet<CasePhoto> CasePhotos { get; set; }
        public DbSet<CaseHistory> CaseHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            // 【修改】預先算好並寫死雜湊值
            var hasher = new PasswordHasher<Employee>();


            // Customers 客戶
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    CustomerId = 1,
                    CustomerName = "T公司",
                    CustomerTaxId = "12345678",
                    CustomerAddress = "台南市新市區",
                    CustomerPhone = "06-111-2222"
                },
                new Customer
                {
                    CustomerId = 2,
                    CustomerName = "發哥",
                    CustomerTaxId = "11223344",
                    CustomerAddress = "台北市",
                    CustomerPhone = "06-111-2233"
                }
            );

            // Plants 廠區
            modelBuilder.Entity<Plant>().HasData(
                new Plant
                {
                    PlantId = 1,
                    PlantName = "新竹廠",
                    PlantCode = "H1",
                    PlantAddress = "新竹市xoxox",
                    PlantPhone = "03-1224545",
                    CustomerId = 1
                },
                new Plant
                {
                    PlantId = 2,
                    PlantName = "台南廠",
                    PlantCode = "N1",
                    PlantAddress = "台南市xoxox",
                    PlantPhone = "06-6548452",
                    CustomerId = 1
                },
                new Plant
                {
                    PlantId = 3,
                    PlantName = "竹科研發中心",
                    PlantCode = "RC1",
                    PlantAddress = "新竹縣xoxox",
                    PlantPhone = "03-2116555",
                    CustomerId = 2
                }
            );

            // Devices 設備
            modelBuilder.Entity<Device>().HasData(
                new Device
                {
                    DeviceId = 1,
                    SerialNumber = "20240901-001",
                    DeviceModel = "NXE3400",
                    ProductionLine = "H1#1",
                    PlantId = 1,
                },
                new Device
                {
                    DeviceId = 2,
                    SerialNumber = "20240901-002",
                    DeviceModel = "NXE3400",
                    ProductionLine = "N1#1",
                    PlantId = 2,
                },
                new Device
                {
                    DeviceId = 3,
                    SerialNumber = "20240901-010",
                    DeviceModel = "ASM9000",
                    ProductionLine = "RC1#1",
                    PlantId = 3,
                }
            );
            // --- 種子資料 (Seed Data) ---
            // 建立要植入的使用者列表
            var employeesToSeed = new List<Employee>
    {
        new Employee
        {
            Id = "1",
            EmployeeName = "王小明",
            HireDate = new DateTime(2022, 1, 1),
            EmployeeTitle = "現場工程師",
            EmployeeAddress = "屏東市xxxxxxxxx",
            EmployeePhone = "0912345678",
            EmergencyContact = "王媽媽",
            EmergencyPhone = "0987654321",
            Status = "在職",
            Role = "Engineer",
            UserName = "user1",
            NormalizedUserName = "USER1",
            EmailConfirmed = true, // 預設確認 Email
            SecurityStamp = Guid.NewGuid().ToString("D") // 產生安全戳記
        },
        new Employee
        {
            Id = "2",
            EmployeeName = "李主管",
            HireDate = new DateTime(2012, 1, 1),
            EmployeeTitle = "業務經理",
            EmployeeAddress = "新竹市xxxxxx",
            EmployeePhone = "0952368741",
            EmergencyContact = "李妻",
            EmergencyPhone = "03-1234567",
            Status = "在職",
            Role = "Manager",
            UserName = "manager1",
            NormalizedUserName = "MANAGER1",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
        new Employee
        {
            Id = "3",
            EmployeeName = "陳大華",
            HireDate = new DateTime(2022, 6, 1),
            EmployeeTitle = "現場工程師",
            EmployeeAddress = "高雄市xxxxxxxxx",
            EmployeePhone = "0919874585",
            EmergencyContact = "陳媽媽",
            EmergencyPhone = "0987654321",
            Status = "在職",
            Role = "Engineer",
            UserName = "user2",
            NormalizedUserName = "USER2",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
        new Employee
        {
            Id = "4",
            EmployeeName = "張經理",
            HireDate = new DateTime(2020, 9, 8),
            EmployeeTitle = "工程部經理",
            EmployeeAddress = "苗栗市xxxxxxxxx",
            EmployeePhone = "0987258678",
            EmergencyContact = "張嬸",
            EmergencyPhone = "0987612587",
            Status = "在職",
            Role = "Manager",
            UserName = "manager2",
            NormalizedUserName = "MANAGER2",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
        new Employee
        {
            Id = "5",
            EmployeeName = "系統管理員",
            HireDate = new DateTime(2025, 9, 5),
            EmployeeTitle = "系統管理員",
            EmployeeAddress = "高雄市xxxxxxxxx",
            EmployeePhone = "0987888888",
            EmergencyContact = "工程師",
            EmergencyPhone = "0987612587",
            Status = "在職",
            Role = "Admin",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
        new Employee
        {
            Id = "6",
            EmployeeName = "LJB",
            Role = "Admin",
            Status = "在職",
            HireDate = new DateTime(2025, 9, 12),
            EmployeeTitle = "系統管理員",
            EmployeeAddress = "高雄市",
            EmployeePhone = "0911111111",
            EmergencyContact = "母",
            EmergencyPhone = "0933333333",
            UserName = "abc",
            NormalizedUserName = "ABC",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        },
        new Employee
        {
            Id = "7",
            EmployeeName = "林人資",
            Role = "HR",
            Status = "在職",
            HireDate = new DateTime(2022, 9, 5),
            EmployeeTitle = "人資",
            EmployeeAddress = "高雄市oooooooo",
            EmployeePhone = "0912365852",
            EmergencyContact = "人資測試",
            EmergencyPhone = "0925856324",
            UserName = "hr_user",
            NormalizedUserName = "HR_USER",
            EmailConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        }
    };

            // 為每位使用者設定雜湊密碼
            employeesToSeed[0].PasswordHash = hasher.HashPassword(employeesToSeed[0], "password");
            employeesToSeed[1].PasswordHash = hasher.HashPassword(employeesToSeed[1], "password");
            employeesToSeed[2].PasswordHash = hasher.HashPassword(employeesToSeed[2], "password");
            employeesToSeed[3].PasswordHash = hasher.HashPassword(employeesToSeed[3], "password");
            employeesToSeed[4].PasswordHash = hasher.HashPassword(employeesToSeed[4], "password");
            employeesToSeed[5].PasswordHash = hasher.HashPassword(employeesToSeed[5], "123456");
            employeesToSeed[6].PasswordHash = hasher.HashPassword(employeesToSeed[6], "password");

            modelBuilder.Entity<Employee>().HasData(employeesToSeed);

            // RepairCases 維修案件
            modelBuilder.Entity<RepairCase>().HasData(
                new RepairCase
                {
                    RepairCaseId = 1,
                    // 【修正】補上 Title 欄位
                    Title = "光刻機光源模組檢查",
                    PlantId = 1,
                    Description = "客戶反應光刻機曝光後晶圓良率降低，需要檢查光源模組",
                    CaseStatus = "OPEN",
                    OccurredAt = new DateTime(2024, 09, 01),
                    DeviceId = 1,
                    EmployeeId = "1",
                },
                new RepairCase
                {
                    RepairCaseId = 2,
                    // 【修正】補上 Title 欄位
                    Title = "蝕刻腔體真空度異常",
                    PlantId = 2,
                    Description = "蝕刻腔體真空無法維持，懷疑是真空幫浦老化",
                    CaseStatus = "暫置",
                    OccurredAt = new DateTime(2024, 09, 02),
                    DeviceId = 2,
                    EmployeeId = "2",
                },
                new RepairCase
                {
                    RepairCaseId = 3,
                    // 【修正】補上 Title 欄位
                    Title = "自動測試程式錯誤",
                    PlantId = 3,
                    Description = "自動測試程式頻繁跳出錯誤代碼，需要檢查控制模組",
                    CaseStatus = "CLOSE",
                    OccurredAt = new DateTime(2024, 09, 03),
                    DeviceId = 3,
                    EmployeeId = "3",
                }

            );
            // 這裡是解決問題的關鍵。
            // 我們手動設定幾個關鍵的關聯，並明確地告訴 Entity Framework，
            // 當父層資料 (例如 Employee) 被刪除時，請不要進行串聯刪除 (ON DELETE NO ACTION)，
            // 而是使用 Restrict (如果還存在關聯的子層資料，就禁止刪除父層資料)。
            // 這樣就打破了多重串聯路徑，解決了 SQL Server 的錯誤。

            // Employee -> RepairCase (一個員工可以建立多個案件)
            modelBuilder.Entity<RepairCase>()
                .HasOne(rc => rc.Employee)
                .WithMany(e => e.RepairCases)
                .HasForeignKey(rc => rc.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> CaseComment (一個員工可以有多個留言)
            modelBuilder.Entity<CaseComment>()
                .HasOne(cc => cc.Employee)
                .WithMany() // 假設 Employee Model 中沒有 ICollection<CaseComment>
                .HasForeignKey(cc => cc.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Employee -> CaseHistory (一個員工可以有多筆歷史紀錄)
            modelBuilder.Entity<CaseHistory>()
                .HasOne(ch => ch.Employee)
                .WithMany() // 假設 Employee Model 中沒有 ICollection<CaseHistory>
                .HasForeignKey(ch => ch.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 規則群組 2：與「廠區/設備」相關的刪除規則 ---

            // 規則 A: 刪除廠區時，連帶刪除其下的所有設備 (Cascade)
            modelBuilder.Entity<Device>()
                .HasOne(d => d.Plant)
                .WithMany(p => p.Devices)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RepairCase>()
               .HasOne(rc => rc.Device)
               .WithMany(d => d.RepairCases)
               .HasForeignKey(rc => rc.DeviceId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RepairCase>()
                .HasOne(rc => rc.Plant)
                .WithMany(p => p.RepairCases)
                .HasForeignKey(rc => rc.PlantId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
    


