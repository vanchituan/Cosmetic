using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel
{
    public class OrderHisViewModel
    {
        public int OrderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhone { get; set; }
        public string PromotionName { get; set; }
        public string ShipName { get; set; }
        public decimal Total { get; set; }

    }
}
