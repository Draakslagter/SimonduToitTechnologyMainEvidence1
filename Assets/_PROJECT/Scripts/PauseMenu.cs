using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private static PauseMenu _instance;
    public static PauseMenu Instance => _instance;
   private bool _isPaused;
   [SerializeField] private CanvasGroup pauseMenuCanvasGroup;

   private void Awake()
   {
       if (_instance != null && _instance != this)
       {
           Destroy(gameObject);
       }
       else
       {
           _instance = this;
       }
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
