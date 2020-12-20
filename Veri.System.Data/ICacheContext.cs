using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Veri.System.Data
{
    public interface ICacheContext
    {
        /// <summary>
        /// Remove item from set
        /// </summary>
        /// <param name="setId">Set identifier</param>
        /// <param name="item">Item in the Set</param>
        void RemoveItemFromSet(string setId, string item);
        IRedisTypedClient<T> GetDb<T>();
        T GetById<T>(object id) where T : IBaseTable;
        void Delete<T>(T entity) where T : IBaseTable;
        void DeleteById<T>(object id);
        void DeleteByIds<T>(IEnumerable ids);
        IList<T> GetAll<T>() where T : IBaseTable;
        IList<T> GetByIds<T>(IEnumerable ids) where T : IBaseTable;
        /// <summary>
        /// Store the change to the context
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="entity">Object instance that contains the data.</param>
        /// <param name="ttl">Time-To-Live in seconds</param>
        /// <returns></returns>
        T Store<T>(T entity, int ttl = -1) where T : IBaseTable;
        void StoreAll<T>(IEnumerable<T> entities, int ttl = -1) where T : IBaseTable;
        IList<TChild> GetRelatedEntities<T, TChild>(object parentId) where T : IBaseTable where TChild : IBaseTable;
        void StoreRelatedEntity<T, TChild>(object parentId, TChild entity) where T : IBaseTable where TChild : IBaseTable;
        void DeleteRelatedEntities<T, TChild>(object parentId) where T : IBaseTable where TChild : IBaseTable;
        void DeleteRelatedEntity<T, TChild>(object parentId, object childToBeDeletedId) where T : IBaseTable where TChild : IBaseTable;
        void StoreRelatedEntities<T, TChild>(object parentId, params TChild[] entities) where T : IBaseTable where TChild : IBaseTable;
    }
}
