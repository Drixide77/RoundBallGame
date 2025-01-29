using UnityEngine;

// Applies DontDestroyOnLoad to the gameobject
public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private bool uniqueInstance = true;

    // Singleton
    public static DontDestroyOnLoad Instance { get; private set; }
    
    private void Awake()
    {
        if (uniqueInstance)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
