using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElektraReport.Applications.DepremKayits.ViewModels
{
    public class VM_DepremKayitDashboard
    {
        public string Name { get; set; }
        public int RoomTotal { get; set; }
        public int CheckOutRoomTotal { get; set; }
        public int UserTotal { get; set; }
        public Guid CompanyId { get; set; }
        public int CompanyTotal { get; set; }
        public bool isCheckOut { get; set; }
    }

    public class VM_DepremDashBoard
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public int MevcutKisi { get; set; }
        public int MevcutOda { get; set; }
        public int CikanOda { get; set; }
        public int CikanKisi { get; set; }
    }

    public class VM_Dashboard
    {
        public List<VM_DepremDashBoard> DepremDashboard { get; set; }
        public int TotalCompany { get; set; }
    }
}
