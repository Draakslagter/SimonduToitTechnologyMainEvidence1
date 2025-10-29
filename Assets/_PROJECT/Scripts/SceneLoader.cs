using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    [SerializeField] private CanvasGroup transitionCanvasGroup;
    [SerializeField] private float transitionDuration = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += HideTransition;
    }

    private static void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SwitchWithTransition(int sceneIndex)
    {
        Time.timeScale = 1.0f;
        transitionCanvasGroup.DOFade(1, transitionDuration).OnComplete(() =>
        {
            UICanvasGroupBehaviour.Instance.ShowActiveCanvasGroup(null);
            SwitchScene(sceneIndex);
        });
    }

    private void HideTransition(Scene previous, Scene current)
    {
        transitionCanvasGroup.DOFade(0, transitionDuration);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}