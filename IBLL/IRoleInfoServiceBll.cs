using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IRoleInfoServiceBll : IBaseServiceBll<RoleInfo>
    {
        bool SetRoleActionInfo(int roleId, List<int> list);
    }
}
