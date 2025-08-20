using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   private bool _isPaused;
   [SerializeField] private CanvasGroup pauseMenuCanvasGroup;

   private void Start()
   {
       PlayerMovementAndControlSetup.Instance.triggerPauseMenu.AddListener(PauseGame);
   }

   public void PauseGame()
    {
        Debug.Log("Paused from Menu");
        _isPaused = !_isPaused;
        pauseMenuCanvasGroup.alpha = _isPaused ? 1 : 0;
        pauseMenuCanvasGroup.blocksRaycasts = !_isPaused;
        pauseMenuCanvasGroup.interactable = !_isPaused;
        Time.timeScale = _isPaused ? 1 : 0;
    }
}
