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
        Music01 = 0,
        
        // SFX (1000 - X)
        Sound01 = 1000,
    }
}