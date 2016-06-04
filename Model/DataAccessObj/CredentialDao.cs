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
    public class CredentialDao
    {
        OnlineShopDBContext db = new OnlineShopDBContext();
        public List<RoleViewModel> ListAllPaging()
        {
            var model = (from a in db.UserGroups
                         join b in db.Credentials on a.Id equals b.UserGroupId
                         join c in db.Roles on b.RoleId equals c.Id
                         select new
                         {
                             roleID = c.Id,
                             roleName = c.Name,
                             userGroupID = a.Id,
                             userGroupName = a.Name
                         }).AsEnumerable()
                        .Select(q => new RoleViewModel()
                        {
                            RoleID = q.roleID,
                            RoleName = q.roleName,
                            UserGroupID = q.userGroupID,
                            UserGroupName = q.userGroupName
                        }).OrderByDescending(q => q.RoleID).ToList();
            return model;
        }

        public bool CheckExists(Credential entity)
        {
            bool result = true;
            // những record có roleID truyền vào có rồi
            var credentialCurrent = db.Credentials.Where(x => x.RoleId == entity.RoleId);
            if (credentialCurrent != null)
            {
                foreach (var item in credentialCurrent)
                {
                    if (item.UserGroupId == entity.UserGroupId)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public void Insert(string userGroupID,string roleID)
        {
            Credential obj = new Credential()
            {
                RoleId = roleID,
                UserGroupId = userGroupID
            };
            db.Credentials.Add(obj);
            db.SaveChanges();
        }

        public bool Delete(string userGroupID,string roleID)
        {
            var cre = db.Credentials.SingleOrDefault(q=>q.RoleId == roleID && q.UserGroupId == userGroupID);
            db.Credentials.Remove(cre);
            db.SaveChanges();
            return true;
        }

    }
}
