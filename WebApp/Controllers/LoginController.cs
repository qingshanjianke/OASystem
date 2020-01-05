using Common;
using IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {

        public IUserInfoServiceBll userInfoService { get; set; }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        #region 显示验证码
        public ActionResult ShowValidateCode()
        {
            ValidateCode validateCode = new ValidateCode();
            string code = validateCode.CreateValidateCode(4);
            Session["validateCode"] = code;
            var buffer = validateCode.CreateValidateGraphic(code);

            return File(buffer, "image/jepg");
        }
        #endregion

        #region 登录验证
        public ActionResult LoginCheck()
        {
            //获取验证码
            String validateCode = Session["validateCode"] != null ? Session["validateCode"].ToString() : string.Empty;
            //清空session
            Session["validateCode"] = null;
            if (String.IsNullOrEmpty(validateCode))
            {
                return Content("验证码生成错误！");
            }
            //用户输入的验证码
            string code = Request["vCode"];
            //判断验证码是否正确；
            if (code.Equals(validateCode,StringComparison.InvariantCultureIgnoreCase))//忽略大小写**
            {
                string txtUName = Request["LoginCode"];
                string TxtUPwd = Request["LoginPwd"];
                var user = userInfoService.LoadEntities(u => u.UName == txtUName && u.UPwd == TxtUPwd).FirstOrDefault();
                if (user != null)
                {
                    //Response.SetCookie("userInfo");

                    string sessionId = Guid.NewGuid().ToString();
                    MemcacheHelper.Set(sessionId, SerializeHelper.SerializeToString(user), DateTime.Now.AddMinutes(20));
                    Response.Cookies["sessionId"].Value = sessionId;//将Memcache的key以Cookie的形式返回给浏览器。
                                                                    //也就是说下一次只要浏览器拿着cookie就能打开memcache取出userinfo对象   
                    Response.Cookies["sessionId"].Expires = DateTime.Now.AddMinutes(20);//如果不设置过期时间的话，关闭浏览器，cookies就会被清除
                    return Content("ok");
                }
                return Content("no");
            }
            else
            {
                return Content("验证码错误！");
            }
        }
        #endregion

    }
}