//using MachineManagementSystemVer2.Models;
//using Microsoft.AspNetCore.Identity;
//using System.Linq;
//using System;
//using System.Collections.Generic;

//namespace MachineManagementSystemVer2.Data
//{
//    public static class DbInitializer
//    {
//        public static void Initialize(AppDbContext context, IPasswordHasher<Employee> passwordHasher)
//        {
//            // 確保資料庫的結構是最新的 (但它不會執行遷移)
//            context.Database.EnsureCreated();

//            // 【關鍵檢查】檢查 Customers 資料表是否已經有任何資料
//            // 如果有，就假設所有種子資料都已建立，直接返回
//            if (context.Customers.Any())
//            {
//                return;
//            }

//            // --- 如果資料庫是空的，就開始新增所有種子資料 ---

//            // 1. Customers
//            var customers = new Customer[]
//            {
//                new Customer { CustomerId = 1, CustomerName = "T公司", CustomerTaxId = "12345678", CustomerAddress = "台南市新市區", CustomerPhone = "06-111-2222" },
//                new Customer { CustomerId = 2, CustomerName = "發哥", CustomerTaxId = "11223344", CustomerAddress = "台北市", CustomerPhone = "06-111-2233" }
//            };
//            context.Customers.AddRange(customers);
//            context.SaveChanges(); // 先儲存客戶，才能在廠區中使用他們的 ID

//            // 2. Plants
//            var plants = new Plant[]
//            {
//                new Plant { PlantId = 1, PlantName = "新竹廠", PlantCode = "H1", PlantAddress = "新竹市xoxox", PlantPhone = "03-1224545", CustomerId = 1 },
//                new Plant { PlantId = 2, PlantName = "台南廠", PlantCode = "N1", PlantAddress = "台南市xoxox", PlantPhone = "06-6548452", CustomerId = 1 },
//                new Plant { PlantId = 3, PlantName = "竹科研發中心", PlantCode = "RC1", PlantAddress = "新竹縣xoxox", PlantPhone = "03-2116555", CustomerId = 2 }
//            };
//            context.Plants.AddRange(plants);
//            context.SaveChanges(); // 先儲存廠區，才能在設備中使用他們的 ID

//            // 3. Devices
//            var devices = new Device[]
//            {
//                 new Device { DeviceId = 1, SerialNumber = "20240901-001", DeviceModel = "NXE3400", ProductionLine = "H1#1", PlantId = 1 },
//                 new Device { DeviceId = 2, SerialNumber = "20240901-002", DeviceModel = "NXE3400", ProductionLine = "N1#1", PlantId = 2 },
//                 new Device { DeviceId = 3, SerialNumber = "20240901-010", DeviceModel = "ASM9000", ProductionLine = "RC1#1", PlantId = 3 }
//            };
//            context.Devices.AddRange(devices);

//            // 4. Employees (with password hashing)
//            var employees = new Employee[]
//            {
//                new Employee { EmployeeName = "王小明", HireDate = new DateTime(2022, 1, 1), EmployeeTitle = "現場工程師", EmployeeAddress = "屏東市xxxxxxxxx", EmployeePhone = "0912345678", EmergencyContact = "王媽媽", EmergencyPhone = "0987654321", Status = "在職", Role = "Engineer", Account = "user1", Password = "password" },
//                new Employee { EmployeeName = "李主管", HireDate = new DateTime(2012, 1, 1), EmployeeTitle = "業務經理", EmployeeAddress = "新竹市xxxxxx", EmployeePhone = "0952368741", EmergencyContact = "李妻", EmergencyPhone = "03-1234567", Status = "在職", Role = "Manager", Account = "manager1", Password = "password" },
//                new Employee { EmployeeName = "陳大華", HireDate = new DateTime(2022, 6, 1), EmployeeTitle = "現場工程師", EmployeeAddress = "高雄市xxxxxxxxx", EmployeePhone = "0919874585", EmergencyContact = "陳媽媽", EmergencyPhone = "0987654321", Status = "在職", Role = "Engineer", Account = "user2", Password = "password" },
//                new Employee { EmployeeName = "張經理", HireDate = new DateTime(2020, 9, 8), EmployeeTitle = "工程部經理", EmployeeAddress = "苗栗市xxxxxxxxx", EmployeePhone = "0987258678", EmergencyContact = "張嬸", EmergencyPhone = "0987612587", Status = "在職", Role = "Manager", Account = "manager2", Password = "password" },
//                new Employee { EmployeeName = "系統管理員", HireDate = new DateTime(2025, 9, 5), EmployeeTitle = "系統管理員", EmployeeAddress = "高雄市xxxxxxxxx", EmployeePhone = "0987888888", EmergencyContact = "工程師", EmergencyPhone = "0987612587", Status = "在職", Role = "Admin", Account="admin", Password= "password"},
//                new Employee { EmployeeName = "LJB", Role = "Admin", Status = "在職", HireDate = new DateTime(2025, 9, 12), EmployeeTitle = "系統管理員", EmployeeAddress = "高雄市", EmployeePhone = "0911111111", EmergencyContact = "母", EmergencyPhone = "0933333333", Account = "abc", Password = "123456" },
//                new Employee { EmployeeName = "林人資", Role = "HR", Status = "在職", HireDate = new DateTime(2022, 9, 5), EmployeeTitle = "人資", EmployeeAddress = "高雄市oooooooo", EmployeePhone = "0912365852", EmergencyContact = "人資測試", EmergencyPhone = "0925856324", Account = "hr_user", Password = "password" }
//            };

//            foreach (Employee e in employees)
//            {
//                // 在新增到 Context 之前，才進行雜湊
//                e.Password = passwordHasher.HashPassword(e, e.Password);
//                context.Employees.Add(e);
//            }

//            // 【最佳化】將所有變更一次性地存入資料庫
//            context.SaveChanges();
//        }
//    }
//}

