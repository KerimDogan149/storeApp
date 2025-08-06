using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Web.Areas.Admin.Models.Reports
{
    public class ReportDashboardViewModel
    {
        public List<MonthlySalesViewModel> MonthlySales { get; set; }
        public List<TopSellingProductViewModel> TopSellers { get; set; }
        public decimal TotalRevenue { get; set; }
        public int? SelectedYear { get; set; }
        public int? SelectedMonth { get; set; }
        public string ChartType { get; set; } = "product";
    public List<string> ChartLabels { get; set; }
    public List<decimal> ChartValues { get; set; }

    }
}