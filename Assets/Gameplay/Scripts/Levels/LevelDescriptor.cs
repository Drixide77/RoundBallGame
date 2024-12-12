using UnityEngine;

namespace RoundBallGame.Gameplay.Levels
{
    public class LevelDescriptor : MonoBehaviour
    {
        public Transform StartPositon;
        public LevelGoalController Goal;
        public DeathTrigger[] DeathTriggers;
        public CannonController[] Cannons;
    }
}
