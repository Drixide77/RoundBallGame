using System;
using UnityEngine;

namespace RoundBallGame.Systems.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "AudioRepository", menuName = "RoundBallGame/Audio/Audio Repository SO", order = 1)]
    public class AudioRepositorySO : ScriptableObject
    {
        public AudioRepositoryEntry[] BGMList;
        public AudioRepositoryEntry[] SFXList;
    }

    [Serializable]
    public class AudioRepositoryEntry
    {
        public AudioRepositoryEntryId Id;
        public AudioClip Clip;
    }

    public enum AudioRepositoryEntryId
    {
        // BGM (0 - 999)
        CompanyLogoMusic = 0,
        MainMenuMusic = 1,
        
        // SFX (1000 - X)
        UIButtonSound = 1000,
        PlayerBounceSound = 1001,
        CannonShotSound = 1002,
        PauseMenuSound = 1003,
        GoalReachedSound = 1004,
        GameOverSound = 1005,
        
    }
}