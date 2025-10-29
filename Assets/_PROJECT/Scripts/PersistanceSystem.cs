using UnityEngine;

public class PersistanceSystem : MonoBehaviour
{
   private void Awake()
   {
      DontDestroyOnLoad(gameObject);
   }
}
