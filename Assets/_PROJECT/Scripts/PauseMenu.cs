using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   private bool _isPaused;
   [SerializeField] private CanvasGroup pauseMenuCanvasGroup;

    // Update is called once per frame
  
    public void PauseGame()
    {
        Debug.Log("Pause Game");
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0 : 1;
        pauseMenuCanvasGroup.alpha = _isPaused ? 1 : 0;
        pauseMenuCanvasGroup.blocksRaycasts = _isPaused;
        pauseMenuCanvasGroup.interactable = _isPaused;
    }
}
