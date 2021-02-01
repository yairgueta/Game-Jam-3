using System.Collections.Generic;
using UnityEngine;

namespace Spawners
{
    public class ObjectPoolManager : Singleton<ObjectPoolManager>
    {
        public List<ObjectPool> pools;

        private static readonly string newPoolName = "[Pool] ";

        protected override void Awake()
        {
            base.Awake();
            pools = new List<ObjectPool>();
        }

        public ObjectPool GetObjectPool(GameObject prefab)
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools[i].PooledPrefab == prefab) return pools[i];
            }
            
            Debug.LogWarning($"Unable to find {prefab?.name} Object Pool! created one insted :(");
            return CreateNewPool(prefab, 1500);
        }


        private ObjectPool CreateNewPool(GameObject prefab, int poolCount)
        {
            GameObject go = new GameObject(newPoolName + prefab.name);
            var pool = go.AddComponent<ObjectPool>();
            pool.InitializePool(prefab, poolCount);
            return pool;
        }
    }
}