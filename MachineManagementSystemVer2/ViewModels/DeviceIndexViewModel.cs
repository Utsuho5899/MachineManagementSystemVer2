using System.Collections.Generic;
using MachineManagementSystemVer2.Models;

namespace MachineManagementSystemVer2.ViewModels
{
    public class DeviceIndexViewModel
    {
        public List<Device> Devices { get; set; } = new List<Device>();
        public string SearchCustomer { get; set; }
        public string SearchPlant { get; set; }
        public string SearchModel { get; set; }

        public Dictionary<string, string> ToRouteDictionary()
        {
            return new Dictionary<string, string>
            {
                { "searchCustomer", SearchCustomer },
                { "searchPlant", SearchPlant },
                { "searchModel", SearchModel }
            };
        }
    }
}