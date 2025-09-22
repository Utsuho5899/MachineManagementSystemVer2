using System;
using System.Collections.Generic;
using MachineManagementSystemVer2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MachineManagementSystemVer2.ViewModels
{
    public class RepairCaseIndexViewModel
    {
        // --- 用於接收篩選條件 ---
        [Display(Name = "案件狀態")]
        public string? SearchStatus { get; set; }

        [Display(Name = "處理人員")]
        public string? SearchEmployeeId { get; set; }

        [Display(Name = "起始日期")]
        public DateTime? SearchStartDate { get; set; }

        [Display(Name = "結束日期")]
        public DateTime? SearchEndDate { get; set; }

        // --- 用於顯示下拉選單 ---
        public SelectList? EmployeeList { get; set; }
        public SelectList? StatusList { get; set; }

        // --- 用於顯示結果 ---
        public List<RepairCase> FilteredCases { get; set; } = new List<RepairCase>();

        // --- 用於顯示摘要數字 ---
        public int TotalCount { get; set; }
        public int OpenCount { get; set; }
        public int ClosedCount { get; set; }

        public Dictionary<string, string> ToRouteDictionary(string sortOrderToRemove)
        {
            return new Dictionary<string, string>
    {
        { "searchStatus", SearchStatus },
        { "searchEmployeeId", SearchEmployeeId?.ToString() },
        { "searchStartDate", SearchStartDate?.ToString("yyyy-MM-dd") },
        { "searchEndDate", SearchEndDate?.ToString("yyyy-MM-dd") }
    };
        }
    };
}
