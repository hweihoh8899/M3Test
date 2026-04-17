using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    private Dictionary<string, GameObject> dictionaryPrefabCache;
    private Dictionary<string, GameObjectPool> dictionaryPool;
    [SerializeField] private Transform root;
    [SerializeField] private int NormalItemPreSpawnQuantity = 40;
    [SerializeField] private int BonusItemPreSpawnQuantity = 4;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        PreloadAllPrefabs();
    }
    
    private void PreloadAllPrefabs()
    {
        dictionaryPrefabCache = new Dictionary<string, GameObject>();
        dictionaryPool = new Dictionary<string, GameObjectPool>();

        CachePrefab(Constants.PREFAB_NORMAL_ITEM, true, NormalItemPreSpawnQuantity);

        CachePrefab(Constants.PREFAB_BONUS_HORIZONTAL, pool: true, preWarm: NormalItemPreSpawnQuantity);
        CachePrefab(Constants.PREFAB_BONUS_VERTICAL, pool: true, preWarm: NormalItemPreSpawnQuantity);
        CachePrefab(Constants.PREFAB_BONUS_BOMB, pool: true, preWarm: NormalItemPreSpawnQuantity);
    }
        
    private void CachePrefab(string path, bool pool, int preWarm)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            return;
        }

        dictionaryPrefabCache[path] = prefab;

        if (pool)
        {
            dictionaryPool[path] = new GameObjectPool(prefab, root, preWarm);
        }
    }

    public GameObject Obtain(string prefabPath)
    {
        if (dictionaryPool.TryGetValue(prefabPath, out GameObjectPool pool))
        {
            return pool.Obtain();
        }

        if (dictionaryPrefabCache.TryGetValue(prefabPath, out GameObject prefab))
        {
            var newPool = new GameObjectPool(prefab, root, 4);
            dictionaryPool[prefabPath] = newPool;
            return newPool.Obtain();
        }

        Debug.LogError("Can't found prefab " + prefabPath);
        return null;
    }
    
    public void Release(string prefabPath, GameObject go)
    {
        if (dictionaryPool.TryGetValue(prefabPath, out GameObjectPool pool))
        {
            pool.Release(go);
        }
        else
        {
            Destroy(go);
        }
    }
    
    public void ClearAllPools()
    {
        foreach (var pool in dictionaryPool.Values) pool.Clear();
        dictionaryPool.Clear();
    }

    private void OnDestroy()
    {
        ClearAllPools();
        Instance = null;
    }
}
