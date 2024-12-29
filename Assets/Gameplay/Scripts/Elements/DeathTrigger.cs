using System;
using UnityEngine;

namespace RoundBallGame.Gameplay.Elements
{
    public class DeathTrigger : MonoBehaviour
    {
        public Action OnDeathTriggerTouched;
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
                    OnDeathTriggerTouched?.Invoke();
                }
            }
        }
    }
}