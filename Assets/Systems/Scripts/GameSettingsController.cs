using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RoundBallGame.Systems.Services;

namespace RoundBallGame.Systems
{
    public class GameSettingsController : MonoBehaviour
    {
        // Singleton pattern
        public static GameSettingsController Instance { get; private set; }
        
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup settingsPanelCanvasGroup;
        [SerializeField] private Toggle volumeToggle;
        [SerializeField] private Toggle sFXToggle;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Button resolutionButton;
        [SerializeField] private TMP_Text resolutionLabel;

        private const string ResolutionIndexKey = "resolution_index";
        
        private int resolutionIndex = 0;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, false);
            volumeToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            sFXToggle.onValueChanged.AddListener(OnSFXToggleChanged);
            fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
            fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
            resolutionButton.interactable = !Screen.fullScreen;
            resolutionButton.onClick.AddListener(OnResolutionButtonPressed);
            
            if (PlayerPrefs.HasKey(ResolutionIndexKey))
            {
                resolutionIndex = PlayerPrefs.GetInt(ResolutionIndexKey);
                AppControlService.Instance.SetResolution(resolutionIndex, Screen.fullScreen);
                resolutionLabel.text = AppControlService.Instance.GetCurrentResolutionMultiplier().ToString(CultureInfo.InvariantCulture);
            };
        }

        private void LateUpdate()
        {
            if (fullscreenToggle.isOn != Screen.fullScreen)
            {
                fullscreenToggle.SetIsOnWithoutNotify(Screen.fullScreen);
                resolutionButton.interactable = !fullscreenToggle.isOn;
            }
        }

        public void ShowSettings()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, true);
        }

        public void HideSettings()
        {
            SetCanvasGroupEnabled(settingsPanelCanvasGroup, false);
        }
        
        private void SetCanvasGroupEnabled(CanvasGroup canvasGroup, bool enabled)
        {
            canvasGroup.alpha = enabled ? 1 : 0;
            canvasGroup.interactable = enabled;
            canvasGroup.blocksRaycasts = enabled;
        }

        private void OnMusicToggleChanged(bool value)
        {
            AudioService.Instance.SetMusicVolume(value ? 1f : 0f);
        }
        
        private void OnSFXToggleChanged(bool value)
        {
            AudioService.Instance.SetSFXVolume(value ? 1f : 0f);
        }
        
        private void OnFullscreenToggleChanged(bool value)
        {
            AppControlService.Instance.SetFullscreen(value);
            fullscreenToggle.SetIsOnWithoutNotify(value);
            resolutionButton.interactable = !fullscreenToggle.isOn;
        }
        
        private void OnResolutionButtonPressed()
        {
            resolutionIndex = (resolutionIndex + 1) % AppControlService.Instance.GetResolutionPresetCount();
            AppControlService.Instance.SetResolution(resolutionIndex);
            resolutionLabel.text = AppControlService.Instance.GetCurrentResolutionMultiplier().ToString();
            
            PlayerPrefs.SetInt(ResolutionIndexKey, resolutionIndex);
        }
    }
}