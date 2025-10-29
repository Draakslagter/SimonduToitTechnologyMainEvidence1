using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool _isPaused;
    private CanvasGroup _pauseMenuCanvasGroup;

    private void Awake()
    {
        if (_pauseMenuCanvasGroup == null)
        {
            _pauseMenuCanvasGroup = GetComponent<CanvasGroup>();
        }
    }
    
    private void Start()
    {
        PlayerMovementAndControlSetup.Instance.triggerPauseMenu.AddListener(PauseGame);
    }

    private void OnDisable()
    {
        PlayerMovementAndControlSetup.Instance.triggerPauseMenu.RemoveListener(PauseGame);
    }

    public void PauseGame()
    {
        _isPaused = !_isPaused;
        UICanvasGroupBehaviour.Instance.ShowActiveCanvasGroup(_isPaused ? _pauseMenuCanvasGroup : null);
        Time.timeScale = _isPaused ? 0 : 1;
    }
}