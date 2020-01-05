using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DALFactory;
using System.Linq.Expressions;

namespace BLL
{
    public abstract class BaseServiceBll<T> where T : class, new()
    {
        public IDBSession CurrentDbSession
        {
            get
            {
                //return new DBSession();
                return DALFactory.DbSessionFactory.CreateDbSession();
            }
        }

        public IDAL.IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseServiceBll()
        {
            SetCurrentDal();
        }

        public T AddEntities(T entity)
        {
            CurrentDal.AddEntities(entity);
            CurrentDbSession.SaveChanges();
            return entity;
        }

        public bool DeleteEntities(T entity) {
            CurrentDal.DeleteEntities(entity);
            return CurrentDbSession.SaveChanges();
        }

        public bool EditEntities(T entity) {
            CurrentDal.EditEntities(entity);
            return CurrentDbSession.SaveChanges();
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda) {
            return CurrentDal.LoadEntities(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<s>(int PageIndex, int PageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {

            return CurrentDal.LoadPageEntities<s>(PageIndex, PageSize,out totalCount, whereLambda, orderbyLambda, isAsc);
        }

    }
}
