using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    private readonly GameObject prefab;
    private readonly Transform root;
    private readonly Stack<GameObject> stackAvailable;
    private readonly HashSet<GameObject> hashSetGotten;

    public GameObjectPool(GameObject prefab, Transform poolRoot, int preSpawnNumber = 0)
    {
        this.prefab = prefab;
        root = poolRoot;
        stackAvailable = new Stack<GameObject>(preSpawnNumber > 0 ? preSpawnNumber : 8);
        hashSetGotten = new HashSet<GameObject>();

        for (int i = 0; i < preSpawnNumber; i++)
        {
            GameObject go = CreateInstance();
            go.SetActive(false);
            stackAvailable.Push(go);
        }
    }

    public GameObject Obtain()
    {
        GameObject go;
        if (stackAvailable.Count > 0)
        {
            go = stackAvailable.Pop();
            go.SetActive(true);
        }
        else
        {
            go = CreateInstance();
        }

        hashSetGotten.Add(go);
        return go;
    }

    public void Release(GameObject go)
    {
        if (go == null) return;
        if (!hashSetGotten.Remove(go)) return;

        go.SetActive(false);
        go.transform.SetParent(root);
        stackAvailable.Push(go);
    }

    public void Clear()
    {
        foreach (var go in stackAvailable)
        {
            if (go != null) Object.Destroy(go);
        }
        stackAvailable.Clear();

        foreach (var go in hashSetGotten)
        {
            if (go != null) Object.Destroy(go);
        }
        hashSetGotten.Clear();
    }

    private GameObject CreateInstance()
    {
        GameObject go = Object.Instantiate(prefab);
        go.transform.SetParent(root);
        return go;
    }
}
