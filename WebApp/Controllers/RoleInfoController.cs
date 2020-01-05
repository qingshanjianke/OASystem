using IBLL;
using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{

    public class RoleInfoController : BaseController
    {
        IRoleInfoServiceBll roleInfoService { get; set; }
        IActionInfoServiceBll actionInfoService { get; set; }

        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 展示角色信息
        public ActionResult GetRoleInfolist()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;//暂时不知道，上午2的18：13 //也要返回给前台，和PageSize计算总页数
            short delFlag = (short)DeleteEnumType.Normal;
            var roleInfoList = roleInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, u => u.DelFlag == delFlag, u => u.ID, true);
            var temp = from u in roleInfoList
                       select new //查询部分列
                       {
                           ID = u.ID,
                           RoleName = u.RoleName,
                           Remark = u.Remark,
                           Sort = u.Sort,
                           SubTime = u.SubTime
                       };
            //easyUI自己就有自己的遍历方式，可以在demo文件夹下的json文件里看到
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);//匿名类型固定
        }
        #endregion

        #region 添加角色信息
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.ModifiedOn = DateTime.Now;
            roleInfo.SubTime = DateTime.Now;
            roleInfo.DelFlag = 0;
            var isSucces = roleInfoService.AddEntities(roleInfo);
            if (isSucces != null)
            {
                return Content("ok:添加成功");
            }
            return Content("no:添加失败");
        }
        #endregion


        #region 展示要选择角色信息
        public ActionResult ShowRoleActionInfo()
        {
            //获得当前的角色信息
            int id = Convert.ToInt32(Request["id"]);
            var roleInfo = roleInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();

            //获得当前角色的权限id
            var actionIdList = (from u in roleInfo.ActionInfo
                                select u.ID).ToList();
            //获得所有权限
            short delFlag = (short)DeleteEnumType.Normal;
            var actionList = actionInfoService.LoadEntities(u => u.DelFlag == delFlag).ToList();
            ViewBag.roleInfo = roleInfo;
            ViewBag.actionIdList = actionIdList;
            ViewBag.actionList = actionList;
            return View();
        }
        #endregion

        #region MyRegion
        public ActionResult SetRoleActionInfo()
        {
            int roleId = Convert.ToInt32(Request["roleId"]);
            string[] actionIdList = Request.Form.AllKeys;
            List<int> list = new List<int>();
            foreach (var actionId in actionIdList)
            {
                if (actionId.Contains("cba_"))
                {
                    var actionID = actionId.Replace("cba_", "");
                    list.Add(Convert.ToInt32(actionID));
                }
            }
            var isSuccess = roleInfoService.SetRoleActionInfo(roleId, list);
            if (isSuccess) {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        #endregion

    }

}