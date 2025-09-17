using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private static PlayerInventory _instance;
    public static PlayerInventory Instance => _instance;
    
    [SerializeField] private int woodCount;
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Update is called once per frame
    public void AddWood(int amount)
    {
        woodCount += amount;
    }

    public bool RemoveWood(int cost)
    {
        if (cost > woodCount)
        {
            return false;
        }
        else
        {
            woodCount -= cost;
            return true;
        }
    }
}
