using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel.Admin.Report
{
    public class ProfitViewModel
    {
        public ListChartDataLine [] chart { get; set; }
        public List<ReportItemLine> table { get; set; }
    }

    public class ReportChartViewModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ReportItemLine
    {
        public DateTime Date { get; set; }
        public decimal? Revenue { get; set; }
        public decimal? Profit { get; set; }
    }
    public class ListChartDataLine
    {
        public ListChartDataLine()
        {
            this.data = new List<ChartDataLine>();
        }
        public string title { get; set; }
        public decimal? max { get; set; }
        public decimal? min { get; set; }
        public List<ChartDataLine> data { get; set; }
    }
    public class ChartDataLine
    {
        public DateTime date { get; set; }
        public decimal value { get; set; }
    }
}
