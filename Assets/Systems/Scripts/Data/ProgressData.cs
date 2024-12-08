using System;

namespace RoundBallGame.Systems.Data
{
    [Serializable]
    public class ProgressData
    {
        public LevelProgressData[] LevelsProgressData;
    }
    
    [Serializable]
    public class LevelProgressData
    {
        public int LevelIndex;
        public bool IsCompleted;
    }
}