 

using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
	
	public partial interface IActionInfoServiceBll : IBaseServiceBll<ActionInfo>
    {
       
    }   
	
	public partial interface IDepartmentServiceBll : IBaseServiceBll<Department>
    {
       
    }   
	
	public partial interface IR_UserInfo_ActionInfoServiceBll : IBaseServiceBll<R_UserInfo_ActionInfo>
    {
       
    }

    public partial interface IRoleInfoServiceBll : IBaseServiceBll<RoleInfo>
    {
       // object SetRoleActionInfo(int roleId, List<int> list);
    }

    public partial interface IUserInfoServiceBll : IBaseServiceBll<UserInfo>
    {
       
    }   
	
}