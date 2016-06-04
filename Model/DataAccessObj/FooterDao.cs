using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class FooterDao
    {
        OnlineShopDBContext myDB = null;
        public FooterDao()
        {
            myDB = new OnlineShopDBContext();
        }

        /// <summary>
        /// lấy ra những footer có status là true
        /// </summary>
        /// <returns>trả về những footer có status la true</returns>
        public Footer GetFooter()
        {
            return myDB.Footers.SingleOrDefault(x => x.Status == true);
        }
    }
}
