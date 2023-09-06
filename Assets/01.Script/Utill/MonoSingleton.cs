using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static bool _shuttingDown = false;
    private static object _locker = new object();
    private static T _instance = null;

    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning("[Instance] Instance" + typeof(T) + "is already destroyed. Returning null.");
                return null;
            }

            lock (_locker)
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        _instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
                        DontDestroyOnLoad(_instance);
                    }
                }

                return _instance;
            }
        }
    }

    protected virtual void Start()
    {
        DontDestroyOnLoad(transform.root);
        _shuttingDown = false;
    }

    private void OnDestroy()
    {
        _shuttingDown = true;
    }

    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }
}