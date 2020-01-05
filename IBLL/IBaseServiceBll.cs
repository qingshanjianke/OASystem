using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public  interface IBaseServiceBll<T> where T :class ,new()
    {
        IDBSession CurrentDbSession { get; }
        IDAL.IBaseDal<T> CurrentDal { get; set; }
        T AddEntities(T entity);
        bool DeleteEntities(T entity);

        bool EditEntities(T entity);
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<s>(int PageIndex, int PageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc);
    }
}
