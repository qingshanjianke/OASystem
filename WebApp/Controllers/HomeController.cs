using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : BaseController
    {
        IBLL.IUserInfoServiceBll userInfoService { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            ViewData["name"] = LoginUser.UName;
            return View();
        }
        public ActionResult CheckLogin()
        {
            return Content("ok");
        }

        #region 获取菜单权限
        public ActionResult getLinks()
        {
            //第一条线的所有权限
            //第二条线的所有权限
            //合并
            //去重
            //将ispass的去掉
            //返回json数据

            //不用BaseController里的loginUser是因为拿出来的是序列化后的数据，在这里就拿不到导航属性了
            var userInfo = userInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
            //1.我的版本（漏了过滤菜单权限类型）
            //var actionInfoFirstList = (from u in userInfo.RoleInfo
            //                 select u.ActionInfo).ToList();

            //1.老师的
            var userRoleInfo = userInfo.RoleInfo;
            var actionTypeEnum = (short)ActionTypeEnum.ActionTypeEnum;
            var actionInfoFirstList = (from u in userRoleInfo
                                       from a in u.ActionInfo
                                       where a.ActionTypeEnum == actionTypeEnum
                                       select a).ToList();
            //2.第二条线的所有权限
            var userActionInfo = from u in userInfo.R_UserInfo_ActionInfo
                                 select u.ActionInfo;

            var actionInfoSecondList = (from u in userActionInfo
                                        where u.ActionTypeEnum == actionTypeEnum
                                        select u).ToList();
            //合并
            actionInfoFirstList.AddRange(actionInfoSecondList);
            
            //将ispass的去掉
            var forbidActions =(from u in userInfo.R_UserInfo_ActionInfo
                         where u.IsPass == false
                         select u.ActionInfo).ToList();

            //复杂做法
            //foreach (var item in actionInfoFirstList)
            //{
            //    if (forbidActions.Contains(item))
            //    {
            //        actionInfoFirstList.Remove(item);
            //    }
            //}
            //简单做法
            var allowActionInfo = actionInfoFirstList.Where(u=>!forbidActions.Contains(u));
            //去重
            var distinctActionInfoList = allowActionInfo.Distinct();

            //返回json数据.
            var temp = from u in distinctActionInfoList
                       select new { icon = u.MenuIcon , title = u.ActionInfoName , url=u.Url };
            return Json(temp,JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}