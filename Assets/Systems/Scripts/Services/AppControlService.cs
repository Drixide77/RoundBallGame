using RoundBallGame.Systems.Data;
using UnityEngine;

namespace RoundBallGame.Systems.Services
{
    public class AppControlService : MonoBehaviour
    {
        // Singleton pattern
        public static AppControlService Instance { get; private set; }
        
        [HideInInspector] public bool firstTimeOnMainMenu;
        
        [SerializeField] private Vector2Int windowedResolution = new (1280, 720);
        [SerializeField] private float[] resolutionMultipliers = { 1f, 2f, 3f };
        private bool fullscreen = false;
        private int resolutionIndex = 0;
        
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

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.FullscreenToggleKey))
            {
                ToggleFullscreen();
            }
        }

        private void Initialize()
        {
            firstTimeOnMainMenu = true;
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
                SetResolution(resolutionIndex);
            }
        }

        public int GetResolutionPresetCount()
        {
            return resolutionMultipliers.Length;
        }

        public float GetCurrentResolutionMultiplier()
        {
            return resolutionMultipliers[resolutionIndex];
        }

        public void SetResolution(int index, bool fullscreen = false)
        {
            resolutionIndex = Mathf.Clamp(index, 0, resolutionMultipliers.Length - 1);
            Screen.SetResolution(Mathf.RoundToInt(windowedResolution.x * resolutionMultipliers[resolutionIndex]), Mathf.RoundToInt(windowedResolution.y * resolutionMultipliers[resolutionIndex]), fullscreen);
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
