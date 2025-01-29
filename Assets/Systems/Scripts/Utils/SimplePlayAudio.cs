using RoundBallGame.Systems.Data;
using RoundBallGame.Systems.Services;
using UnityEngine;

namespace RoundBallGame.Systems.Utils
{
    public class SimplePlayAudio : MonoBehaviour
    {
        public enum AudioType
        {
            BGM, SFX
        }
        
        [Header("Settings")]
        [SerializeField] private AudioRepositoryEntryId clipToPlay;
        [SerializeField] private AudioType auidoType;
        [SerializeField] private bool loop = false;

        private void Start()
        {
            switch (auidoType)
            {
                case AudioType.BGM:
                    AudioService.Instance.PlayMusicClip(clipToPlay, loop);
                    break;
                case AudioType.SFX:
                    AudioService.Instance.PlaySFXClip(clipToPlay, loop);
                    break;
                default:
                    break;
            }
        }
    }
}