using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel.Client.Cart
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? OrderDate { get; set; }

        public int? CustomerId { get; set; }

        public string ShipName { get; set; }


        public string ShipMobile { get; set; }


        public string ShipAddress { get; set; }


        public string ShipEmail { get; set; }

        public decimal? Total { get; set; }

        public int PromotionId { get; set; }

        public int Status { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? PrecinctId { get; set; }

        public string BankCode { get; set; }

        public string PaymentMethod { get; set; }
    }
}
