using System;
using UnityEngine;

namespace RoundBallGame.Gameplay.Levels
{
    public class LevelGoalController : MonoBehaviour
    {
        public Action OnGoalReached;
        private bool hasBeenTriggered = false;

        public void Initialize()
        {
            hasBeenTriggered = false;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (!hasBeenTriggered)
                {
                    hasBeenTriggered = true;
                    OnGoalReached?.Invoke();
                }
            }
        }
    }
}
