using UnityEngine;
using UnityEngine.Events;

public class PoolObject : MonoBehaviour
{
    public GameObject prefab;
    public UnityEvent onInit = new();
    public UnityEvent onReturn = new();

    private bool _isInit;

    public void Init()
    {
        if (_isInit) return;
        foreach (var component in GetComponentsInChildren<MonoBehaviour>())
        {
            // 풀 오브젝트가 인잇, 리턴때 실행되는 이벤트 여부
            if (component is IInitActionAble initAble) onInit.AddListener(initAble.OnInit);
            if (component is IReturnAction returnable) onReturn.AddListener(returnable.OnReturn);
        }

        _isInit = true;
    }

    private void Awake()
    {
        Init();
    }
}