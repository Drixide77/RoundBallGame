using System;
using RoundBallGame.Gameplay.Levels;
using RoundBallGame.Systems;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace RoundBallGame.Gameplay
{
    public class LevelSceneController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform playerParent;
        [Space(10)]
        [Header("UI")]
        [SerializeField] private PauseEndScreenController pauseEndScreenController;
        [Space(10)]
        [Header("Levels")]
        [SerializeField] private LevelCollectionSO levelCollection;
        [SerializeField] private Transform levelParent;
        [FormerlySerializedAs("levelScene")]
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private SceneAsset mainMenuScene;

        private LevelDescriptor currentLevelInstance;
        private PlayerBallController playerInstance;

        // Unity Methods
        private void Awake()
        {
            playerInstance = Instantiate(playerPrefab,  Vector3.zero, Quaternion.identity, playerParent).GetComponent<PlayerBallController>();
            playerInstance.Initialize();
            playerInstance.Hide();
            AddEventCallbacks();
        }

        private void Start()
        {
            InitializeLevel();
            StartLevel();
        }

        private void OnDestroy()
        {
            CleanUpLevel();
            RemoveEventCallbacks();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                HandlePauseInput();
            }
        }

        private void HandlePauseInput()
        {
            if (pauseEndScreenController.isShown)
            {
                if (pauseEndScreenController.currentType == PauseEndScreenController.PauseEndScreenType.Pause) OnResumeLevel();
            }
            else
            {
                OnPauseLevel();
            }
        }

        private void StartLevel()
        {
            currentLevelInstance.Goal.Initialize();
            playerInstance.Initialize();
            playerInstance.MoveTo(currentLevelInstance.StartPositon.position);
            playerInstance.Show();
            Time.timeScale = 1f;
        }
        
        private void InitializeLevel()
        {
            int currentLevelIndex = DataService.Instance.CurrentLevelIndex;
            currentLevelInstance = Instantiate(levelCollection.Levels[currentLevelIndex].gameObject, Vector3.zero, Quaternion.identity, levelParent).GetComponent<LevelDescriptor>();
            currentLevelInstance.Goal.OnGoalReached += OnGoalReached;
        }
        
        private void CleanUpLevel()
        {
            if (currentLevelInstance == null) return;
            currentLevelInstance.Goal.OnGoalReached -= OnGoalReached;
            Destroy(currentLevelInstance.gameObject);
        }
        
        // Event Callbacks
        private void AddEventCallbacks()
        {
            pauseEndScreenController.OnNextLevelButtonClicked += OnNextLevelSelected;
            pauseEndScreenController.OnResumeButtonClicked += OnResumeLevel;
            pauseEndScreenController.OnRetryButtonClicked += OnRestartLevel;
            pauseEndScreenController.OnMenuButtonClicked += OnQuitToMenu;
        }
        
        private void RemoveEventCallbacks()
        {
            pauseEndScreenController.OnNextLevelButtonClicked -= OnNextLevelSelected;
            pauseEndScreenController.OnResumeButtonClicked -= OnResumeLevel;
            pauseEndScreenController.OnRetryButtonClicked -= OnRestartLevel;
            pauseEndScreenController.OnMenuButtonClicked -= OnQuitToMenu;
        }
        
        private void OnGoalReached()
        {
            playerInstance.Hide();
            pauseEndScreenController.Show(PauseEndScreenController.PauseEndScreenType.LevelComplete);
        }

        private void OnPauseLevel()
        {
            Time.timeScale = 0f;
            pauseEndScreenController.Show(PauseEndScreenController.PauseEndScreenType.Pause);
        }
        
        private void OnResumeLevel()
        {
            pauseEndScreenController.Hide();
            Time.timeScale = 1f;
        }
        
        private void OnRestartLevel()
        {
            pauseEndScreenController.Hide();
            StartLevel();
        }

        private void OnNextLevelSelected()
        {
            CleanUpLevel();
            DataService.Instance.CurrentLevelIndex++;
            pauseEndScreenController.Hide();
            InitializeLevel();
            StartLevel();
        }

        private void OnQuitToMenu()
        {
            CleanUpLevel();
            SceneManager.LoadSceneAsync(mainMenuScene.name, LoadSceneMode.Single);
        }
    }
}
