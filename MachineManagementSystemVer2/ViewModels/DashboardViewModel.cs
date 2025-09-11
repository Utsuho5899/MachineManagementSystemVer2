using System.Collections.Generic;
using MachineManagementSystemVer2.Models;

namespace MachineManagementSystemVer2.ViewModels
{
    public class DashboardViewModel
    {
        public int OpenCasesCount { get; set; }
        public int ClosedCasesCount { get; set; }
        public int TotalDevicesCount { get; set; }
        public int TotalCustomersCount { get; set; }
        public List<RepairCase> RecentCases { get; set; } = new List<RepairCase>();
    }
}

