using BLL;
using Common;
using IBLL;
using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class UserInfoController : BaseController//Controller
    {
        //UserInfoServiceBll有的 他的接口肯定也得有啊
        IUserInfoServiceBll userInfoService { get; set; }
        IRoleInfoServiceBll roleInfoService { get; set; }
        IActionInfoServiceBll actionInfoService { get; set; }
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 获取用户列表数据
        public ActionResult GetUserInfolist()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;//暂时不知道，上午2的18：13 //也要返回给前台，和PageSize计算总页数
            short delFlag = (short)DeleteEnumType.Normal;
            var UserInfoList = userInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, u => u.DelFlag == delFlag, c => c.ID, true);
            var temp = from u in UserInfoList
                       select new
                       {
                           ID = u.ID,
                           UName = u.UName,
                           UPwd = u.UPwd,
                           Remark = u.Remark,
                           SubTime = u.SubTime
                       };
            return Json(new { rows = temp, total = totalCount });//匿名类型固定
        }
        #endregion

        #region 删除多行记录
        public ActionResult DeleteUserInfo()
        {
            String strId = Request["strID"];
            String[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            for (int i = 0; i < strIds.Length; i++)
            {
                list.Add(Convert.ToInt32(strIds[i]));
            }
            if (userInfoService.DeleteUserInfoList(list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 添加用户
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            userInfo.SubTime = DateTime.Now;
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfoService.AddEntities(userInfo);
            return Content("ok");
        }
        #endregion

        #region 显示用户数据
        public ActionResult ShowUserInfo()
        {
            int id = int.Parse(Request["strId"]);
            UserInfo user = userInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            //return Json(user, JsonRequestBehavior.AllowGet);
            //JObject jo = JObject.Parse(strJson);//将Json字符串转为JObject类型，后续可方便直接取值
            string jsonString = SerializeHelper.SerializeToString(user);
            JsonResult js = Json(jsonString, JsonRequestBehavior.AllowGet);
            return js;
        }

        #endregion

        #region 修改用户数据
        public ActionResult EditUserInfo(UserInfo userInfo)
        {
            userInfo.ModifiedOn = DateTime.Now;
            userInfoService.EditEntities(userInfo);
            return Content("ok");
        }
        #endregion

        #region 获取用户角色数据
        public ActionResult ShowUserRoleInfo()
        {
            int id = int.Parse(Request["id"]);
            var userInfo = userInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //查询所有的角色.
            short delFlag = (short)DeleteEnumType.Normal;
            var allRoleList = roleInfoService.LoadEntities(r => r.DelFlag == delFlag).ToList();
            //查询一下要分配角色的用户以前具有了哪些角色编号。
            var allUserRoleIdList = (from r in userInfo.RoleInfo
                                     select r.ID).ToList();
            ViewBag.AllRoleList = allRoleList;
            ViewBag.AllUserRoleIdList = allUserRoleIdList;
            return View();
        }
        #endregion

        #region 完成用户角色的分配
        public ActionResult SetUserRoleInfo()
        {
            int userId = int.Parse(Request["userId"]);
            string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性值。
            List<int> roleIdList = new List<int>();
            foreach (string key in allKeys)
            {
                Console.WriteLine(key);
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_", "");
                    roleIdList.Add(Convert.ToInt32(k));
                }
            }
            if (userInfoService.SetUserRoleInfo(userId, roleIdList))//设置用户的角色
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 为用户分配权限
        public ActionResult ShowUserActionInfo()
        {
            //获取选中用户的信息
            int id = Convert.ToInt32(Request["id"]);
            var userInfo = userInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.userInfo = userInfo;
            //获取权限列表
            short delFlag = (short)DeleteEnumType.Normal;
            var actionList = actionInfoService.LoadEntities(u => u.DelFlag == delFlag).ToList();
            ViewBag.actionList = actionList;
            //获取该用户拥有的权限
            var userActionList = (from u in userInfo.R_UserInfo_ActionInfo
                                  select u
                                  ).ToList();
            ViewBag.userActionList = userActionList;
            return View();
        }
        #endregion

        

        #region changeRadio
        public ActionResult changeRadio()
        {
            int actionId = int.Parse(Request["actionId"]);
            int UserId = int.Parse(Request["UserId"]);
            bool isPass = Request["isPass"] == "true" ? true : false;
            var issuccess = userInfoService.SetUserActionInfo(actionId,UserId,isPass);
            if (issuccess)
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

        #region 清除radio
        public ActionResult ClearRadio() {
            int actionId = int.Parse(Request["actionId"]);
            int UserId = int.Parse(Request["UserId"]);
            var isSuccess = userInfoService.ClearUserActionInfo(actionId, UserId);
            if (isSuccess)
            {
                return Content("ok");
            }
            else {
                return Content("no");
            }

        }
        #endregion

        

    }
}