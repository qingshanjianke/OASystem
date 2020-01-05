using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial class RoleInfoServiceBll : IRoleInfoServiceBll
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="list">被选中的权限ID</param>
        /// <returns></returns>
        public bool SetRoleActionInfo(int roleId, List<int> list) {
            var roleInfo = this.CurrentDbSession.RoleInfoDal.LoadEntities(u=>u.ID ==roleId).FirstOrDefault();
            if (roleInfo!= null) {
                roleInfo.ActionInfo.Clear();
                foreach (var actionID in list)
                {
                    var actionInfo = this.CurrentDbSession.ActionInfoDal.LoadEntities(u=>u.ID == actionID).FirstOrDefault();
                    roleInfo.ActionInfo.Add(actionInfo);
                }
            }
            return CurrentDbSession.SaveChanges();
        }
    }
}
