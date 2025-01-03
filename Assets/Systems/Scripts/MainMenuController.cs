using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RoundBallGame.Systems
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup mainPanelCanvasGroup;
        [SerializeField] private CanvasGroup buttonContainerCanvasGroup;
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private CanvasGroup levelSelectPanelCanvasGroup;
        [SerializeField] private Button levelSelectBackButton;
        [SerializeField] private RectTransform levelButtonsParent;
        [Space(10)]
        [Header("Animation Settings")]
        [SerializeField] private float fadeInDelay;
        [SerializeField] private float alphaStepAmount;
        [SerializeField] private float stepTime;
        [SerializeField] private float holdTime;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private GameObject levelButtonPrefab;
        [SerializeField] private string levelSceneName;

        private Coroutine animationCoroutine;
        
        private void Awake()
        {
            Time.timeScale = 1f; // Making sure so it doesn't affect the animation coroutine

            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            SetCanvasGroupEnabled(buttonContainerCanvasGroup, true);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            SetCanvasGroupEnabled(levelSelectPanelCanvasGroup, false);
            levelSelectBackButton.onClick.AddListener(OnLevelSelectBackButtonClicked);
        }
        
        private void Start()
        {
            // Done on Start to ensure AppControlService has been initialized
            if (AppControlService.Instance.firstTimeOnMainMenu)
            {
                SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
                SetCanvasGroupEnabled(buttonContainerCanvasGroup, false);
                animationCoroutine = StartCoroutine(AnimateMenuCoroutine());
                AppControlService.Instance.firstTimeOnMainMenu = false;
            }

            // Done on Start to ensure DataService has been initialized
            CreateLevelButtons();
        }
        
        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            levelSelectBackButton.onClick.RemoveAllListeners();
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }
        
        private void OnPlayButtonClicked()
        {
            SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
            SetCanvasGroupEnabled(levelSelectPanelCanvasGroup, true);
        }
        
        private void OnExitButtonClicked()
        {
            AppControlService.Instance.ExitApplication();
        }

        private void OnLevelSelectBackButtonClicked()
        {
            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            SetCanvasGroupEnabled(levelSelectPanelCanvasGroup, false);
        }
        
        private void SetCanvasGroupEnabled(CanvasGroup canvasGroup, bool enabled)
        {
            canvasGroup.alpha = enabled ? 1 : 0;
            canvasGroup.interactable = enabled;
            canvasGroup.blocksRaycasts = enabled;
        }
        
        private void CreateLevelButtons()
        {
            for (int i = 0; i < DataService.Instance.GetLevelCollection().Levels.Length; i++)
            {
                LevelButtonController levelButton = Instantiate(levelButtonPrefab, levelButtonsParent).GetComponent<LevelButtonController>();
                levelButton.SetButtonInfo(i, DataService.Instance.GetLevelProgress(i), levelSceneName);
            }
        }
        
        private IEnumerator AnimateMenuCoroutine()
        {
            yield return new WaitForSeconds(fadeInDelay);
            while (mainPanelCanvasGroup.alpha < 1f)
            {
                mainPanelCanvasGroup.alpha += alphaStepAmount;
                yield return new WaitForSeconds(stepTime);
            }
            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            yield return new WaitForSeconds(holdTime);
            SetCanvasGroupEnabled(buttonContainerCanvasGroup, true);
        }
    }
}
