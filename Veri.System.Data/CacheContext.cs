using ServiceStack;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Veri.System.Data
{
    public class CacheContext : ICacheContext
    {
        protected RedisManagerPool pool;
        protected IRedisClient redis;


        public CacheContext(string cacheUri, string license, int maxPoolSize = 4)
        {
            Licensing.RegisterLicense(license);
            pool = new RedisManagerPool(
                cacheUri,
                new RedisPoolConfig
                {
                    MaxPoolSize = maxPoolSize
                });
        }

        public void RemoveItemFromSet(string setId, string item)
        {
            using (var client = pool.GetClient())
            {
                client.RemoveItemFromSet(setId, item);
            }
        }

        public IRedisTypedClient<T> GetDb<T>()
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                return db;
            }
        }

        public T GetById<T>(object id) where T : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                IBaseTable t = client.GetById<T>(id);
                if (t != null)
                {
                    t.Cache = this;
                }
                return (T)t;
            }
        }

        public void Delete<T>(T entity) where T : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.Delete(entity);
            }
        }

        public void DeleteById<T>(object id)
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.DeleteById(id);
            }
        }

        public void DeleteByIds<T>(IEnumerable ids)
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.DeleteByIds(ids);
            }
        }

        public IList<T> GetAll<T>() where T : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                IList<T> baseTable = db.GetAll();
                baseTable.ToList().ForEach(i => i.Cache = this);
                return baseTable;
            }
        }

        public IList<T> GetByIds<T>(IEnumerable ids) where T : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                IList<T> baseTable = db.GetByIds(ids);
                baseTable.ToList().ForEach(i => i.Cache = this);
                return baseTable;
            }
        }

        public T Store<T>(T entity, int ttl = -1) where T : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                if (ttl != -1)
                {
                    return db.Store(entity, TimeSpan.FromMinutes(ttl));
                }
                else
                {
                    return db.Store(entity);
                }
            }
        }

        public void StoreAll<T>(IEnumerable<T> entities, int ttl = -1) where T : IBaseTable
        {
            if (ttl != -1)
            {
                foreach (var entity in entities)
                {
                    Store(entity, ttl);
                }
            }
            else
            {
                using (var client = pool.GetClient())
                {
                    var db = client.As<T>();
                    db.StoreAll(entities);
                }
            }
        }
        public IList<TChild> GetRelatedEntities<T, TChild>(object parentId)
            where T : IBaseTable
            where TChild : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                return db.GetRelatedEntities<TChild>(parentId);
            }
        }


        public void DeleteRelatedEntities<T, TChild>(object parentId)
            where T : IBaseTable
            where TChild : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.DeleteRelatedEntities<TChild>(parentId);
            }
        }

        public void StoreRelatedEntity<T, TChild>(object parentId, TChild entity)
            where T : IBaseTable
            where TChild : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.StoreRelatedEntities(parentId, entity);
            }
        }

        public void StoreRelatedEntities<T, TChild>(object parentId, params TChild[] entities)
            where T : IBaseTable
            where TChild : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.StoreRelatedEntities(parentId, entities);
            }
        }

        public void DeleteRelatedEntity<T, TChild>(object parentId, object childToBeDeletedId)
            where T : IBaseTable
            where TChild : IBaseTable
        {
            using (var client = pool.GetClient())
            {
                var db = client.As<T>();
                db.DeleteRelatedEntity<TChild>(parentId, childToBeDeletedId);
            }
        }
    }
}
