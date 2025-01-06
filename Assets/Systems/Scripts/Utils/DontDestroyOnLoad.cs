using UnityEngine;

// Applies DontDestroyOnLoad to the gameobject
public class DontDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
