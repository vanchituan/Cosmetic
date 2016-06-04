using DataLayer.Framework;
using DataLayer.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataAccessObj
{
    public class RoleDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public List<Role> ListAll()
        {
            return db.Roles.ToList();
        }

        public void Insert(Role entity)
        {
            Role r = new Role()
            {
                Id = entity.Id,
                Name = entity.Name
            };
            db.Roles.Add(entity);
            db.SaveChanges();
        }

        public List<UserGroup> ListUserGroup()
        {
            return db.UserGroups.ToList();
        }

        public List<Role> ListRole()
        {
            return db.Roles.ToList();
        }

        public bool CheckExist(string id)
        {
            bool result = true;
            if(db.Roles.Any(m=>m.Id == id))
            {
                result = false;
            }
            return result;
        }

        public void Delete(string id)
        {
            var role = db.Roles.Find(id);
            db.Roles.Remove(role);
            db.SaveChanges();
        }

        public void Edit(string id)
        {
            var currentRole = db.Roles.Find(id);
            Role r = new Role();
            r.Id = currentRole.Id;
            r.Name = currentRole.Name;
            db.SaveChanges();
        }

    }
}
