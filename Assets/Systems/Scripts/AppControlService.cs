using System;
using RoundBallGame.Systems.Data;
using UnityEngine;

namespace RoundBallGame.Systems
{
    public class AppControlService : MonoBehaviour
    {
        // Singleton pattern
        public static AppControlService Instance { get; private set; }
        
        [SerializeField] private Vector2Int windowedResolution = new Vector2Int(1280, 720);
        private bool fullscreen = false;

        public bool firstTimeOnMainMenu = true;
        
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

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.FullscreenToggleKey))
            {
                ToggleFullscreen();
            }
        }
        
        public void ToggleFullscreen()
        {
            fullscreen = !Screen.fullScreen;
            SetFullscreen(fullscreen);
        }

        public void SetFullscreen(bool fullscreen)
        {
            this.fullscreen = fullscreen;
            if (this.fullscreen)
            {
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            }
            else
            {
                Screen.SetResolution(windowedResolution.x, windowedResolution.y, false);
            }
        }

        public void ExitApplication()
        {
            Debug.Log("Exiting game...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
