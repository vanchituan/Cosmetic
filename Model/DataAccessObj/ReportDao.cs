using DataLayer.Framework;
using DataLayer.ViewModel.Admin.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class ReportDao
    {
        public ProfitViewModel GetDataLine(DateTime? fromDate, DateTime? toDate)
        {
            var db = new OnlineShopDBContext();
            var fDate = fromDate.HasValue ? fromDate.Value.Date : DateTime.Now.AddMonths(-1).Date;
            var tDate = toDate.HasValue ? toDate.Value.AddDays(1).Date : DateTime.Now.AddDays(1).Date;
            ProfitViewModel result = new ProfitViewModel();
            List<ListChartDataLine> list = new List<ListChartDataLine>();
            result.table = new List<ReportItemLine>();
            try
            {
                var data = (from a in db.Invoices
                           where a.CreatedDate >= fromDate && a.CreatedDate <= toDate
                           select new ReportItemLine
                           {
                               Date = a.CreatedDate.Value,
                              Revenue = a.Total,
                              Profit = a.Profit
                           })
                           .GroupBy(q=> new
                           {
                               Date = q.Date
                           })
                           .Select(g => new
                           {
                               Date = g.Key.Date,
                               Revenue = g.Sum(s=>s.Revenue),
                               Profit = g.Sum(s=>s.Profit)
                           }).ToList();

                if (data.Any())
                {
                    var dateTemp = data[0].Date.Date;
                    var Revenue = data[0].Revenue;
                    var Profit = data[0].Profit;

                    var REV = new ListChartDataLine();
                    REV.title = "Doanh thu";
                    var PRO = new ListChartDataLine();
                    PRO.title = "Lợi nhuận";


                    for (int i = 1; i < data.Count; i++)
                    {
                        if (dateTemp == data[i].Date.Date)
                        {
                            Revenue += data[i].Revenue;
                            Profit += data[i].Profit;
                        }
                        else
                        {
                            REV.data.Add(new ChartDataLine() { date = dateTemp, value = Revenue.Value });
                            PRO.data.Add(new ChartDataLine() { date = dateTemp, value = Profit.Value });
                            result.table.Add(new ReportItemLine
                            {
                                Date = dateTemp,
                                Revenue = Revenue,
                                Profit = Profit,
                            });
                            dateTemp = data[i].Date.Date;
                            Revenue = data[i].Revenue;
                            Profit = data[i].Profit;

                        }
                    }
                    REV.data.Add(new ChartDataLine() { date = dateTemp, value = Revenue.Value });
                    PRO.data.Add(new ChartDataLine() { date = dateTemp, value = Profit.Value });

                    result.table.Add(new ReportItemLine
                    {
                        Date = dateTemp,
                        Revenue = Revenue,
                        Profit = Profit
                    });

                    PRO.min = PRO.data.Min(m => m.value);
                    PRO.max = PRO.data.Max(m => m.value);
                    REV.min = REV.data.Min(m => m.value);
                    REV.max = REV.data.Max(m => m.value);

                    list.Add(REV);
                    list.Add(PRO);

                }
                result.chart = list.ToArray();
            }
            catch (Exception ex)
            {
                db.Dispose();
            }
            return result;
        }

        public IEnumerable<Invoice> GetDateDetail(DateTime date)
        {
            var db = new OnlineShopDBContext();
            var model = from a in db.Invoices
                        where a.CreatedDate.Value.Day == date.Day && 
                        a.CreatedDate.Value.Month == date.Month &&
                        a.CreatedDate.Value.Year == date.Year
                        select a;
            return model;
        }
    }
}
