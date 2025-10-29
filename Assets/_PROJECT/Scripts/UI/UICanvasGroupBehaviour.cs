using UnityEngine;

public class UICanvasGroupBehaviour : MonoBehaviour
{
   public static UICanvasGroupBehaviour Instance;
   [SerializeField] private CanvasGroup[] uiCanvasGroups;

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

   public void ShowActiveCanvasGroup(CanvasGroup canvasGroup)
   {
      HideCanvasGroups();
      if (canvasGroup == null) return;
      canvasGroup.alpha = 1;
      canvasGroup.blocksRaycasts = true;
      canvasGroup.interactable = true;
   }

   private void HideCanvasGroups()
   {
      foreach (var canvasGroup in uiCanvasGroups)
      {
         canvasGroup.alpha = 0;
         canvasGroup.blocksRaycasts = false;
         canvasGroup.interactable = false;
      }
   }
}
