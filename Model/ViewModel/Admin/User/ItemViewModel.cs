using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel.Admin.User
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }

        public string FullAddress { get; set; }
        public int PromotionId { get; set; }
        public decimal Point{ get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int PrecinctId { get; set; }
        public bool Status { get; set; }
        public string PromotionName { get; set; }
        public string GroupName { get; set; }
    }
}
