using IBLL;
using IDAL;
using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public partial class UserInfoServiceBll : BaseServiceBll<UserInfo>, IUserInfoServiceBll
    {
        public bool DeleteUserInfoList(List<int> list)
        {
            var userInfoList = CurrentDal.LoadEntities(u => list.Contains(u.ID));
            foreach (var userInfo in userInfoList)
            {
                CurrentDal.DeleteEntities(userInfo);
            }
            return CurrentDbSession.SaveChanges();
        }
        public bool SetUserRoleInfo(int userId, List<int> roleIdList)
        {
            //找到userID的用户，再通过roleIDList将用户的角色改了（添加或者删除）
            var userinfo = this.CurrentDal.LoadEntities(u => u.ID == userId).FirstOrDefault();
            if (userinfo != null)
            {
                userinfo.RoleInfo.Clear();
                foreach (var roleId in roleIdList)
                {
                    var role = this.CurrentDbSession.RoleInfoDal.LoadEntities(u => u.ID == roleId).FirstOrDefault();
                    userinfo.RoleInfo.Add(role);
                }
            }
            return CurrentDbSession.SaveChanges();

        }
        #region 实现radio按钮的禁止允许
        /// <summary>
        /// 实现radio按钮的禁止允许
        /// </summary>
        /// <param name="actionId">权限编号</param>
        /// <param name="userId">用户编号</param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public bool SetUserActionInfo(int actionId, int userId, bool isPass)
        {
            //var userInfo = this.CurrentDbSession.UserInfoDal.LoadEntities(u=>u.ID==userId).FirstOrDefault();
            //var isExt = (from a in userInfo.R_UserInfo_ActionInfo
            //            where a.ActionInfoID == actionId
            //            select a).FirstOrDefault();

            var isExt = this.CurrentDbSession.R_UserInfo_ActionInfoDal.LoadEntities(u => u.UserInfoID == userId && u.ActionInfoID == actionId).FirstOrDefault();
            if (isExt != null)
            {
                isExt.IsPass = isPass;
                this.CurrentDbSession.R_UserInfo_ActionInfoDal.EditEntities(isExt);
            }
            else
            {
                R_UserInfo_ActionInfo r_UserInfo_ActionInfo = new R_UserInfo_ActionInfo();
                r_UserInfo_ActionInfo.ActionInfoID = actionId;
                r_UserInfo_ActionInfo.UserInfoID = userId;
                r_UserInfo_ActionInfo.IsPass = isPass;
                this.CurrentDbSession.R_UserInfo_ActionInfoDal.AddEntities(r_UserInfo_ActionInfo);
            }
            return CurrentDbSession.SaveChanges();
        }
        #endregion

        public bool ClearUserActionInfo(int actionId, int userId) {
            var userActionInfo = this.CurrentDbSession.R_UserInfo_ActionInfoDal.LoadEntities(u=>u.ActionInfoID ==actionId&&u.UserInfoID==userId).FirstOrDefault();
            if (userActionInfo!=null) {
                this.CurrentDbSession.R_UserInfo_ActionInfoDal.DeleteEntities(userActionInfo);
            }
            else
            {
                return false;
            }

            return CurrentDbSession.SaveChanges();
        }

    }
}
