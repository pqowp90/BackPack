using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolStorage : MonoBehaviour
{
    private static readonly Dictionary<GameObject, Stack<GameObject>> PooledObjects = new();
    private static readonly Dictionary<GameObject, List<GameObject>> RegisteredObjects = new();
    private static readonly Dictionary<GameObject, PoolObject> PoolObjects = new();

    private static Transform _defaultParent;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        var go = new GameObject("PoolManager");
        go.AddComponent<PoolStorage>();
        go.hideFlags = HideFlags.HideInHierarchy;

        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(go);
    }

    private static void OnSceneUnloaded(Scene _)
    {
        PooledObjects.Clear();
        RegisteredObjects.Clear();

        _defaultParent = null;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _defaultParent = new GameObject("PoolObjects").transform;
    }

    public static GameObject InstantiateObject(GameObject prefab, Transform parent = null)
    {
        parent ??= _defaultParent;

        GameObject InitGameObject()
        {
            var instance = Instantiate(prefab, parent);
            RegisterObject(instance, prefab);
            if (instance.TryGetComponent(out PoolObject _)) return instance;
            var poolObject = instance.AddComponent<PoolObject>();
            poolObject.prefab = prefab;
            poolObject.Init();
            PoolObjects.TryAdd(instance, poolObject);
            return instance;
        }

        return InitGameObject();

        // if (!prefab.TryGetComponent(out NetworkIdentity _)) return InitGameObject();

        // if (!NetManager.Instance.isServer) return null;
        // var instance = InitGameObject();
        // NetworkServer.Spawn(instance);
        // return instance;
        // 여기서 스폰을 할 일은 없을듯.
    }

    private static void RegisterObject(GameObject instance, GameObject prefab, bool isPooled = false)
    {
        if (!RegisteredObjects.ContainsKey(prefab)) RegisteredObjects.Add(prefab, new List<GameObject>());

        instance.name = $"{prefab.name} [{RegisteredObjects[prefab].Count}]";
        RegisteredObjects[prefab].Add(instance);

        if (isPooled) instance.SetActive(false);
    }

    public static GameObject GetObject(GameObject prefab, Transform parent = null)
    {
        parent ??= _defaultParent;

        if (!PooledObjects.ContainsKey(prefab)) PooledObjects.Add(prefab, new Stack<GameObject>());

        var instance = PooledObjects[prefab].Count > 0
            ? PooledObjects[prefab].Pop()
            : InstantiateObject(prefab, parent);
        instance.transform.SetParent(parent);
        instance.SetActive(true);
        var poolObject = PoolObjects[instance];
        poolObject.onInit.Invoke();
        instance.hideFlags = HideFlags.None;
        return instance;
    }

    public static void ReturnObject(GameObject instance)
    {
        if (PoolObjects.TryGetValue(instance, out var poolObject))
        {
            poolObject.onReturn.Invoke();
        }
        else
        {
            Debug.LogWarning("Prefab not found for instance " + instance.name);
            Destroy(instance);
            return;
        }

        var prefab = poolObject.prefab;
        if (prefab is null)
        {
            Debug.LogWarning("Prefab not found for instance " + instance.name);
            Destroy(instance);
            return;
        }

        if (!PooledObjects.ContainsKey(prefab)) PooledObjects.Add(prefab, new Stack<GameObject>());

        PooledObjects[prefab].Push(instance);
        instance.SetActive(false);
        //instance.hideFlags = HideFlags.HideInHierarchy;
    }
}