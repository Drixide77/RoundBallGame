using UnityEngine;

namespace RoundBallGame.Systems
{
    public class DataService : MonoBehaviour
    {
        // Singleton pattern
        public static DataService Instance;
        private DataService _instance;

        public int CurrentLevelIndex = -1;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
