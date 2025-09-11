using System;
using System.Collections.Generic;
using MachineManagementSystemVer2.Models;

namespace MachineManagementSystemVer2.ViewModels
{
    public class RepairCaseDetailViewModel
    {
        // 案件基本資訊
        public int RepairCaseId { get; set; }
        public string CaseStatus { get; set; }
        public DateTime OccurredAt { get; set; }
        public string? CustomerContact { get; set; }
        public string Description { get; set; }
        public string? CaseRemark { get; set; }

        // 關聯物件的名稱
        public string PlantName { get; set; }
        public string DeviceName { get; set; }
        public string EmployeeName { get; set; }


        // 關聯的留言與照片
        public List<CaseComment> Comments { get; set; } = new List<CaseComment>();
        public List<CasePhoto> Photos { get; set; } = new List<CasePhoto>();

        // 用於新增留言的表單模型
        public string NewCommentContent { get; set; }
    }
}