using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModel
{
    public class TagDetailViewModel
    {
        OnlineShopDBContext db = new OnlineShopDBContext();

        public string TagID { get; set; }
        public string TagName { get; set; }
        public int ContentID { get; set; }
        public string ContentName { get; set; }
    }
}
