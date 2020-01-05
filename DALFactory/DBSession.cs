using DAl;
using IDAL;
using Modal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALFactory
{
    public partial class DBSession : IDBSession
    {
        //public IDAL.IUserInfoDal CreateUserInfoDAL() {
        //    return new DAl.UserInfoDal();
        //}

        //OAEntities db = new OAEntities();


        // DbContext db = DBContextFactory.CreateDbContext();
        //public DbContext db {
        //    get {
        //        if (db == null) { 
        //            db = DBContextFactory.CreateDbContext();
        //        } 
        //    }
        //}
        public DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }

        }
        //private IUserInfoDal _UserInfoDal;
        //public IUserInfoDal UserInfoDal
        //{
        //    get
        //    {
        //        if (_UserInfoDal == null)
        //        {
        //            _UserInfoDal = new UserInfoDal();
        //        }
        //        return _UserInfoDal;
        //    }
        //    set
        //    {
        //        _UserInfoDal = value;
        //    }
        //}
        /// <summary>
        /// 一个业务中经常涉及到对多张表操作，我们希望连接一次数据库，完成对多张表数据的操作，提高性能
        /// 工作单元模式
        /// </summary>
        /// <returns></returns>
        public Boolean SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
