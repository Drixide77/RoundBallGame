using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace RoundBallGame.Systems
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private CanvasGroup mainPanelCanvasGroup;
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private CanvasGroup levelSelectPanelCanvasGroup;
        [SerializeField] private Button levelSelectBackButton;
        [SerializeField] private RectTransform levelButtonsParent;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private GameObject levelButtonPrefab;
        [SerializeField] private SceneAsset levelScene;
        
        private void Awake()
        {
            SetCanvasGroupEnabled(mainPanelCanvasGroup, true);
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
            SetCanvasGroupEnabled(levelSelectPanelCanvasGroup, false);
            levelSelectBackButton.onClick.AddListener(OnLevelSelectBackButtonClicked);
        }
        
        private void Start()
        {
            // Done on Start to ensure DataService has been initialized
            CreateLevelButtons();
        }
        
        private void OnDestroy()
        {
            playButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            levelSelectBackButton.onClick.RemoveAllListeners();
        }
        
        private void OnPlayButtonClicked()
        {
            SetCanvasGroupEnabled(mainPanelCanvasGroup, false);
            SetCanvasGroupEnabled(levelSelectPanelCanvasGroup, true);
        }
        
        private void OnExitButtonClicked()
        {
            Debug.Log("Exiting game...");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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
                levelButton.SetButtonInfo(i, DataService.Instance.GetLevelProgress(i), levelScene.name);
            }
        }
    }
}
