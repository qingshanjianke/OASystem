using Common;
using IBLL;
using Modal;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        //IUserInfoServiceBll userInfoService { get; set; }
        //IActionInfoServiceBll actionInfoService { get; set; }
        public UserInfo LoginUser { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);

            Boolean isSucess = false;
            if (Request.Cookies["sessionId"] != null)
            {
                //通过cookie取出memcache的key
                var sessionId = Request.Cookies["sessionId"].Value;
                //这个时候因为memcache有过期时间，所以这里还要判断一下这个用户在memcache中还存在不
                if (MemcacheHelper.Get(sessionId) != null)
                {
                    //用key从memcache中取出该用户的信息
                    var userInfo = SerializeHelper.DeserializeToObject<UserInfo>(MemcacheHelper.Get(sessionId).ToString());
                    isSucess = true;
                    LoginUser = userInfo;
                    //模拟滑动过期时间
                    MemcacheHelper.Set(sessionId, SerializeHelper.SerializeToString(LoginUser), DateTime.Now.AddMinutes(20));

                    //校验非菜单权限
                    var httpUrl = Request.Url.AbsolutePath;
                    var method = Request.HttpMethod;

                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IUserInfoServiceBll userInfoService = (IUserInfoServiceBll)ctx.GetObject("UserInfoServiceBll");
                    IActionInfoServiceBll actionInfoService = (IActionInfoServiceBll)ctx.GetObject("ActionInfoServiceBll");
                    //开发专用
                    if (LoginUser.UName=="Admin")
                    {
                        return;
                    }
                    //获取请求路径对应的权限信息
                    var actionInfo = actionInfoService.LoadEntities(u => u.HttpMethod == method && u.Url == httpUrl).FirstOrDefault();
                    //获取用户信息
                    var userLoginInfo = userInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
                    //判断用户是否具有该权限

                    //用户—权限
                    var isExt = (from u in userLoginInfo.R_UserInfo_ActionInfo
                    where u.ActionInfoID == actionInfo.ID
                    select u).FirstOrDefault();
                    if (isExt != null)//有这条权限，但未必可用
                    {
                        if (isExt.IsPass)
                        {
                            return;
                        }
                        else
                        {
                             filterContext.Result = Redirect("/Error.Html");
                            return;
                        }
                    }
                    else//没有这条权限走第二条线看看有没有这个权限
                    {
                        var roleInfo = userLoginInfo.RoleInfo;
                        var Count = (from u in roleInfo
                                           from a in u.ActionInfo
                                           where a.ID == actionInfo.ID
                                           select a).Count();
                        if (Count>0)
                        {
                            return;
                        }
                        else
                        {
                            filterContext.Result = Redirect("/Error.Html");
                            return;
                        }

                    }

                }
            }

            if (!isSucess)
            {
                //filterContext.HttpContext.Response.Redirect("/Login/Index");
                filterContext.Result = Redirect("/Login/Index");
            }
        }
    }
}