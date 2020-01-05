using Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IUserInfoServiceBll : IBaseServiceBll<UserInfo>
    {
        bool DeleteUserInfoList(List<int> list);
        bool SetUserRoleInfo(int userId, List<int> roleIdList);
        bool SetUserActionInfo(int actionId, int userId, bool isPass);

        bool ClearUserActionInfo(int actionId, int userId);
    }
}
