using System.Collections.Generic;
using RoundBallGame.Systems.Data;
using UnityEngine;

namespace RoundBallGame.Systems.Services
{
    public class AudioService : MonoBehaviour
    {
        // Singleton
        public static AudioService Instance { get; private set; }

        [Header("Assets")]
        [SerializeField] private AudioRepositorySO audioRepository;
        [Header("Components")]
        [SerializeField] private AudioSource musicAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;

        private Dictionary<AudioRepositoryEntryId, AudioClip> _musicDictionary;
        private Dictionary<AudioRepositoryEntryId, AudioClip> _sfxDictionary;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Initialize()
        {
            _musicDictionary = new Dictionary<AudioRepositoryEntryId, AudioClip>();
            foreach (var musicEntry in audioRepository.BGMList)
            {
                _musicDictionary.Add(musicEntry.Id, musicEntry.Clip);
            }
            _sfxDictionary = new Dictionary<AudioRepositoryEntryId, AudioClip>();
            foreach (var sfxEntry in audioRepository.SFXList)
            {
                _sfxDictionary.Add(sfxEntry.Id, sfxEntry.Clip);
            }
        }

        public void PlayMusicClip(AudioClip clipToPlay, bool loop = false)
        {
            musicAudioSource.loop = loop;
            musicAudioSource.clip = clipToPlay;
            musicAudioSource.Play();
        }
        
        public void PlayMusicClip(AudioRepositoryEntryId Id, bool loop = false)
        {
            musicAudioSource.loop = loop;
            musicAudioSource.clip = _musicDictionary[Id];
            musicAudioSource.Play();
        }

        public void PlaySFXClip(AudioClip clipToPlay, bool loop = false)
        {
            sfxAudioSource.loop = loop;
            sfxAudioSource.PlayOneShot(clipToPlay);
        }
        
        public void PlaySFXClip(AudioRepositoryEntryId Id, bool loop = false)
        {
            sfxAudioSource.loop = loop;
            sfxAudioSource.PlayOneShot(_sfxDictionary[Id]);
        }

        public void StopMusic()
        {
            musicAudioSource.Stop();
        }
        
        public void PauseMusic()
        {
            musicAudioSource.Pause();
        }
        
        public void ResumeMusic()
        {
            musicAudioSource.Play();
        }
        
        public void StopSfx()
        {
            sfxAudioSource.Stop();
        }
        
        public void PauseSfx()
        {
            sfxAudioSource.Pause();
        }
        
        public void ResumeSfx()
        {
            sfxAudioSource.Play();
        }
        
        public void SetMusicVolume(float normalizedVolume)
        {
            musicAudioSource.volume = normalizedVolume;
        }

        public void SetSFXVolume(float normalizedVolume)
        {
            sfxAudioSource.volume = normalizedVolume;
        }
    }
}