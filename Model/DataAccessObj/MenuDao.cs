using DataLayer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class MenuDao
    {
        OnlineShopDBContext db = null;
        public MenuDao()
        {
            db = new OnlineShopDBContext();
        }


        /// <summary>
        /// danh sách menu có nhóm ID là tham số truyền vào
        /// </summary>
        /// <param name="groupID">groupID của menu</param>
        /// <returns>trả về danh sách </returns>
        public List<Menu> ListByGroupID(int groupID)
        {
            return db.Menus.Where(x => x.TypeID == groupID && x.Status == true).OrderBy(x =>x.DisplayOrder).ToList();
        }
    }
}
