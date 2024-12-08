using System;
using RoundBallGame.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoundBallGame.Gameplay
{
    public class PauseEndScreenController : MonoBehaviour
    {
        public enum PauseEndScreenType
        {
            Pause,
            LevelComplete,
            GameOver
        }
        
        public Action OnResumeButtonClicked;
        public Action OnNextLevelButtonClicked;
        public Action OnRetryButtonClicked;
        public Action OnMenuButtonClicked;
        public bool IsShown = false;
        public PauseEndScreenType currentType = PauseEndScreenType.Pause;
        
        [Header("UI Elements")]
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button menuButton;
        
        private const string PauseScreenMessage = "Pause";
        private const string LevelCompleteScreenMessage = "Level Completed!";
        private const string GameOverScreenMessage = "Try again...";
        
        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            IsShown = false;
            
            resumeButton.onClick.AddListener(() => OnResumeButtonClicked?.Invoke());
            nextLevelButton.onClick.AddListener(() => OnNextLevelButtonClicked?.Invoke());
            retryButton.onClick.AddListener(() => OnRetryButtonClicked?.Invoke());
            menuButton.onClick.AddListener(() => OnMenuButtonClicked?.Invoke());
        }

        private void OnDestroy()
        {
            resumeButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.RemoveAllListeners();
            retryButton.onClick.RemoveAllListeners();
            menuButton.onClick.RemoveAllListeners();
        }

        public void Show(PauseEndScreenType type)
        {
            switch (type)
            {
                case PauseEndScreenType.Pause:
                    messageText.text = PauseScreenMessage;
                    resumeButton.gameObject.SetActive(true);
                    nextLevelButton.gameObject.SetActive(false);
                    retryButton.gameObject.SetActive(true);
                    menuButton.gameObject.SetActive(true);
                    break;
                case PauseEndScreenType.LevelComplete:
                    messageText.text = LevelCompleteScreenMessage;
                    resumeButton.gameObject.SetActive(false);
                    nextLevelButton.gameObject.SetActive(DataService.Instance.NextLevelExists());
                    retryButton.gameObject.SetActive(true);
                    menuButton.gameObject.SetActive(true);
                    break;
                case PauseEndScreenType.GameOver:
                    messageText.text = GameOverScreenMessage;
                    resumeButton.gameObject.SetActive(false);
                    nextLevelButton.gameObject.SetActive(false);
                    retryButton.gameObject.SetActive(true);
                    menuButton.gameObject.SetActive(true);
                    break;
            }
            currentType = type;
            IsShown = true;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
        }
        
        public void Hide()
        {
            IsShown = false;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
        }
    }
}
