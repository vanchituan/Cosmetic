using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class SlideDao
    {
        OnlineShopDBContext db = null;

        public SlideDao()
        {
            db = new OnlineShopDBContext();
        }


        /// <summary>
        /// danh sách các slide
        /// </summary>
        /// <returns>1 record có status là true và sắp xếp theo display order</returns>
        public List<Slide> ListAll()
        {
            return db.Slides.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }
    }
}
