using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text tipText;
    [SerializeField] private float delayTime = 0.5f;
    [SerializeField] private float transitionTime = 0.5f;

    [SerializeField] private GameTipSO gameTip;
    [SerializeField] private List<GameObject> characters = new List<GameObject>(); 

    private void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0;
        camera.gameObject.SetActive(Camera.main == null);
        DontDestroyOnLoad(gameObject);
    }

    private void Load()
    {
        SetRandomTip();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        if (_targetScene == null) return;
        StartCoroutine(LoadSceneCoroutine());
    }

    private void OnSceneUnloaded(Scene scene)
    {
        camera.gameObject.SetActive(Camera.main == null);
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void SetRandomTip()
    {
        tipText.text = gameTip.GameTips[Random.Range(0, gameTip.GameTips.Count)];
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        camera.gameObject.SetActive(Camera.main == null);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator ShowLoadingScreenCoroutine()
    {
        foreach (var item in characters)
        {
            item.SetActive(false);
        }
        characters[Random.Range(0, characters.Count)].SetActive(true);
        canvasGroup.DOFade(1, transitionTime);
        yield return new WaitForSeconds(transitionTime);
    }

    private IEnumerator HideLoadingScreenCoroutine()
    {
        canvasGroup.DOFade(0, transitionTime);
        yield return new WaitForSeconds(transitionTime);
    }

    private IEnumerator LoadSceneCoroutine()
    {
        yield return StartCoroutine(ShowLoadingScreenCoroutine());

        // SceneManager.UnloadSceneAsync(_currentScene);

        yield return new WaitForSeconds(delayTime);

        if (_targetScene != null)
        {
            var asyncOperation = SceneManager.LoadSceneAsync(_targetScene);
            asyncOperation.completed += _ => IsDone = true;

            var timer = 0.0f;
            while (!asyncOperation.isDone)
            {
                yield return null;

                timer += Time.deltaTime;

                if (!(asyncOperation.progress >= 0.9f)) continue;

                if (timer >= transitionTime)
                    asyncOperation.allowSceneActivation = true;
            }
        }

        yield return StartCoroutine(HideLoadingScreenCoroutine());
        _targetScene = null;
    }

    #region static

    private static SceneLoader Instance { get; set; }
    public static bool IsLoading => _targetScene != null;

    public static bool IsDone { get; set; }

    private static Scene _currentScene;
    private static string _targetScene;

    public static void LoadScene(string scene)
    {
        _currentScene = SceneManager.GetActiveScene();
        _targetScene = scene;
        Instance.Load();
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
    }

    #endregion
}