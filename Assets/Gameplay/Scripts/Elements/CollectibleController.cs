using System;
using UnityEngine;

namespace RoundBallGame.Gameplay.Levels
{
    public class CollectibleController : MonoBehaviour
    {
        [SerializeField] private int collectibleIndex;
        
        public Action<int> OnCollectibleCollected;
        private bool hasBeenTriggered = false;

        public void Initialize()
        {
            hasBeenTriggered = false;
        }
        
        // Collisions
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!hasBeenTriggered)
                {
                    hasBeenTriggered = true;
                    OnCollectibleCollected?.Invoke(collectibleIndex);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
