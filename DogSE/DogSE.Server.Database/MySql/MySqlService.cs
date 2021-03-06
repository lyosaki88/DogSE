﻿using System.Data;
using DogSE.Server.Common;
using IvyOrm;
using MySql.Data.MySqlClient;

namespace DogSE.Server.Database.MySQL
{

    /// <summary>
    /// Mysql对数据访问接口的实现
    /// </summary>
    public class MySqlService : IDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectStr"></param>
        public MySqlService(string connectStr)
        {
            conPool = new MySqlConnectPool();
            conPool.ConnectString = connectStr;
        }

        private readonly MySqlConnectPool conPool;

        #region IDataService Members

        public T LoadEntity<T>(int serial) where T : class,  IDataEntity,new()
        {
            var con = conPool.GetConnection();
            var sql = string.Format("select * from {0} where id = {1}", typeof (T).Name, serial);
            var ret = con.RecordSingleOrDefault<T>(sql);
            conPool.ReleaseContent(con);
            return ret;
        }

        public T[] LoadEntitys<T>() where T : class,  IDataEntity, new()
        {
            var con = conPool.GetConnection();
            var sql = string.Format("select * from {0}", typeof(T).Name);
            var ret = con.RecordQuery<T>(sql);
            conPool.ReleaseContent(con);
            return ret;
        }

        
        public int UpdateEntity<T>(T entity) where T : class,  IDataEntity
        {
            var con = conPool.GetConnection();
            con.RecordUpdate(entity);
            conPool.ReleaseContent(con);

            return 1;
        }



        public int InsertEntity<T>(T entity) where T : class,  IDataEntity
        {
            var con = conPool.GetConnection();
            con.RecordInsert(entity);
            conPool.ReleaseContent(con);
            return 1;
        }

        public int DeleteEntity<T>(T entity) where T : class, IDataEntity
        {
            var con = conPool.GetConnection();
            con.RecordDelete(entity);
            conPool.ReleaseContent(con);

            return 1;
        }

        public int ExecuteSql(string sql)
        {
            var con = conPool.GetConnection();
            var ret = con.ExecuteNonQuery(sql);
            conPool.ReleaseContent(con);
            return ret;
        }

        public DataSet ExecuteDataSet(string sql)
        {
            var con = conPool.GetConnection();
            var ret = MySqlHelper.ExecuteDataset(con, sql);
            conPool.ReleaseContent(con);
            return ret;
        }

        #endregion
    }

}