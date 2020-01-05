using Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAl
{
    public class BaseDal<T> where T:class,new()
    {
        //public DbContext db {
        //    get {
        //        return DBContextFactory.CreateDbContext();
        //    }

        //}
        DbContext Db = DBContextFactory.CreateDbContext();
        public T AddEntities(T entity)
        {
            Db.Set<T>().Add(entity);
            //db.SaveChanges();
            return entity;
        }

        public bool DeleteEntities(T entity)
        {
            Db.Set<T>().Remove(entity);
            //return db.SaveChanges() > 0;
            return true;
        }

        public bool EditEntities(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            //return db.SaveChanges() > 0;
            return true;
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where<T>(whereLambda);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="s"></typeparam>
        /// <param name="PageIndex">页码</param>
        /// <param name="PageSize">页面数</param>
        /// <param name="totalCount">查询总数</param>
        /// <param name="whereLambda"></param>
        /// <param name="orderbyLambda"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<s>(int PageIndex, int PageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = Db.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>(PageSize * (PageIndex - 1)).Take<T>(PageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>(PageSize * (PageIndex - 1)).Take<T>(PageSize);
            }
            return temp;
        }
    }
}
