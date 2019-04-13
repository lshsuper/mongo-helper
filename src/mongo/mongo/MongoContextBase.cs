using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace mongo
{
    public class MongoContextBase
    {
        private static MongoClient _client;
        private static object _lockForMongo = new object();
        /// <summary>
        /// 对外实例
        /// </summary>
        public static MongoClient GetInstance
        {

            get
            {
                if (_client != null)
                {
                    return _client;
                }

                lock (_lockForMongo)
                {
                    if (_client != null)
                    {
                        return _client;
                    }
                    _client = new MongoClient();
                    return _client;
                }
            }
        }
        #region Basic CURD
        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static T Find<T>(Expression<Func<T, bool>> where, string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            return tb.Find<T>(where).FirstOrDefault<T>();
        }
        /// <summary>
        /// 获取列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(Expression<Func<T, bool>> where, string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            return tb.Find<T>(where).ToEnumerable<T>();
        }
        /// <summary>
        /// 获取分页列表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static IEnumerable<T> Page<T>(Expression<Func<T, bool>> where,int pageSize,int pageIndex,string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            return tb.Find<T>(where).Skip((pageIndex-1)*pageSize).Limit(pageSize).ToEnumerable<T>();
        }
        /// <summary>
        /// 删除单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool Remove<T>(Expression<Func<T, bool>> where, string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            var result = tb.DeleteOne<T>(where);
            return result.IsAcknowledged;
        }
        /// <summary>
        /// 删除(批量)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool RemoveMany<T>(Expression<Func<T, bool>> where, string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            var result = tb.DeleteMany<T>(where);
            return result.IsAcknowledged;
        }
        /// <summary>
        /// 更新单条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="fields"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool Modify<T>(Expression<Func<T, bool>> where, IDictionary<Expression<Func<T,object>>,object>fields, string database, string table) where T:class,new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            var build = new UpdateDefinitionBuilder<T>();
            UpdateDefinition<T> model = null;
            foreach (var field in fields)
            {
                model=build.Set<Object>(field.Key,field.Value);
            }
            if (model == null)
            {
                return false;
            }
            var result=tb.UpdateOne<T>(where,model);
            return result.IsAcknowledged;
        }
        /// <summary>
        /// 更新（批量）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <param name="fields"></param>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool ModifyMany<T>(Expression<Func<T, bool>> where, IDictionary<Expression<Func<T, object>>, object> fields, string database, string table) where T : class, new()
        {
            var db = GetInstance.GetDatabase(database);
            var tb = db.GetCollection<T>(table);
            var build = new UpdateDefinitionBuilder<T>();
            UpdateDefinition<T> model = null;
            foreach (var field in fields)
            {
                model = build.Set<Object>(field.Key, field.Value);
            }
            if (model == null)
            {
                return false;
            }
            var result = tb.UpdateMany<T>(where, model);
            return result.IsAcknowledged;
        }
        #endregion
    }
}
