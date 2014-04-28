using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;

namespace MVCBase.Core.Cache
{
    public class CacheHelper
    {
        private ObjectCache _mcache;
        public CacheHelper()
        {
            this._mcache = MemoryCache.Default;
        }

         public void Set<T>(string key, T data)
        {
            System.DateTimeOffset absoluteExpiration = DateTimeOffset.Now.AddMonths(1);
            //Set(key, data, absoluteExpiration);
            //if (data != null)
            if (!Equals(data,default(T)))
                Set<T>(key, data, absoluteExpiration);
        }
        public void Set<T>(string key, T data, System.DateTimeOffset absoluteExpiration, string regionName = null)
        {
            CacheItem item = new CacheItem(key, data, regionName);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = absoluteExpiration;
            Set(item, policy);
        }
        public void Set(CacheItem item, CacheItemPolicy policy)
        {
            if (item == null || item.Value == null)
                return;
            if (policy != null && policy.ChangeMonitors != null && policy.ChangeMonitors.Count > 0)
                throw new NotSupportedException("Change monitors are not supported");

            // max timeout in scaleout = 65535 
            //TimeSpan expire = (policy.AbsoluteExpiration.Equals(null)) ?
            //policy.SlidingExpiration :
            //(policy.AbsoluteExpiration - DateTimeOffset.Now);
            //double timeout = expire.TotalMinutes;
            //if (timeout > 65535)
            //    timeout = 65535;
            //else if (timeout > 0 && timeout < 1)
            //    timeout = 1;

            MemoryCache.Default.Add(item, policy);
            //this._client.Store(Enyim.Caching.Memcached.StoreMode.Set, item.Key.ToString(), item.Value);
        }

        public T Get<T>(string key)
        {
            return (T)_mcache[key];
        }

        public object Get(string key, string regionName = null)
        {
            return _mcache[key];
        }
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Set(key, value, null);
            }
        }
        public object AddOrGetExisting(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            CacheItem item = GetCacheItem(key, regionName);
            if (item == null)
            {
                Set(new CacheItem(key, value, regionName), policy);
                return value;
            }
            return item.Value;
        }
        public CacheItem AddOrGetExisting(CacheItem value, CacheItemPolicy policy)
        {
            CacheItem item = GetCacheItem(value.Key, value.RegionName);
            if (item == null)
            {
                Set(value, policy);
                return value;
            }
            return item;
        }
        public object AddOrGetExisting(string key, object value, System.DateTimeOffset absoluteExpiration, string regionName = null)
        {
            CacheItem item = new CacheItem(key, value, regionName);
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = absoluteExpiration;
            return AddOrGetExisting(item, policy);
        }
        public bool Contains(string key, string regionName = null)
        {
            if (this.Get(key) == null)
                return false;
            else
                return true;
        }
        //public CacheEntryChangeMonitor CreateCacheEntryChangeMonitor(System.Collections.Generic.IEnumerable<string> keys, string regionName = null)
        //{
        //    throw new System.NotImplementedException();
        //}
        //public DefaultCacheCapabilities DefaultCacheCapabilities
        //{
        //    get
        //    {
        //        return
        //        DefaultCacheCapabilities.OutOfProcessProvider |
        //        DefaultCacheCapabilities.AbsoluteExpirations |
        //        DefaultCacheCapabilities.SlidingExpirations |
        //        DefaultCacheCapabilities.CacheRegions;
        //    }
        //}
        public CacheItem GetCacheItem(string key, string regionName = null)
        {
            object value = Get(key, regionName);
            if (value != null)
                return new CacheItem(key, value, regionName);
            return null;
        }
        public long GetCount(string regionName = null)
        {
            return this._mcache.GetCount();
        }
        //protected override System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<string, object>> GetEnumerator()
        //{
        //    throw new System.NotImplementedException();
        //}
        //public override System.Collections.Generic.IDictionary<string, object> GetValues(System.Collections.Generic.IEnumerable<string> keys, string regionName = null)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public override string Name
        //{
        //    get { return "MemcachedProvider"; }
        //}

        public object Remove(string key, string regionName = null)
        {
            return _mcache.Remove(key);
            //return this._client.Remove(key);
        }

        public void Set(string key, object value, CacheItemPolicy policy, string regionName = null)
        {
            Set(new CacheItem(key, value, regionName), policy);
        }

        //#region ICacheBuilder Members
        //public ObjectCache GetInstance()
        //{
        //    return this;
        //}
        //public string DefaultRegionName
        //{
        //    get { throw new NotImplementedException(); }
        //}
        //#endregion
    }
}
