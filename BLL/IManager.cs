 
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
	
	public partial class ActionInfoServiceBll :BaseServiceBll<ActionInfo>,IActionInfoServiceBll
    {
    

		 public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDbSession.ActionInfoDal;
        }
    }   
	
	public partial class DepartmentServiceBll :BaseServiceBll<Department>,IDepartmentServiceBll
    {
    

		 public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDbSession.DepartmentDal;
        }
    }   
	
	public partial class R_UserInfo_ActionInfoServiceBll :BaseServiceBll<R_UserInfo_ActionInfo>,IR_UserInfo_ActionInfoServiceBll
    {
    

		 public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDbSession.R_UserInfo_ActionInfoDal;
        }
    }   
	
	public partial class RoleInfoServiceBll :BaseServiceBll<RoleInfo>,IRoleInfoServiceBll
    {
    

		 public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDbSession.RoleInfoDal;
        }
    }   
	
	public partial class UserInfoServiceBll :BaseServiceBll<UserInfo>,IUserInfoServiceBll
    {
    

		 public override void SetCurrentDal()
        {
            CurrentDal = this.CurrentDbSession.UserInfoDal;
        }
    }   
	
}