using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public interface IBaseDal<T> where T:class ,new()
    {
        IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="whereLambda">查询</param>
        /// <param name="orderbyLambda">排序</param>
        /// <returns></returns>
        IQueryable<T> LoadPageEntities<s>(int PageIndex, int PageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> whereLambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, Boolean isAsc);
        //添加
        T AddEntities(T entity);
        //删除
        Boolean DeleteEntities(T entity);
        //更新
        Boolean EditEntities(T entity);
    }
}
