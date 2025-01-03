using RoundBallGame.Gameplay.Camera;
using RoundBallGame.Gameplay.Elements;
using RoundBallGame.Gameplay.Levels;
using RoundBallGame.Systems;
using RoundBallGame.Systems.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        [SerializeField] private Transform levelParent;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;
        [Space(10)]
        [Header("Controllers")]
        [SerializeField] private CameraController cameraController;

        private LevelCollectionSO levelCollection;
        private LevelDescriptor currentLevelInstance;
        private PlayerBallController playerInstance;

        // Unity Methods
        private void Awake()
        {
            playerInstance = Instantiate(playerPrefab,  Vector3.zero, Quaternion.identity, playerParent).GetComponent<PlayerBallController>();
            playerInstance.Initialize();
            playerInstance.Hide();
            levelCollection = DataService.Instance.GetLevelCollection();
            cameraController.SetTarget(playerInstance.transform);
            AddEventCallbacks();
        }

        private void Start()
        {
            InitializeLevel();
            StartLevel();
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
            CleanUpLevel();
            RemoveEventCallbacks();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeybindingsDefinition.PauseKey1) || Input.GetKeyDown(KeybindingsDefinition.PauseKey1))
            {
                HandlePauseInput();
            }

            if (Input.GetKeyDown(KeybindingsDefinition.RestartLevelKey))
            {
                OnRestartLevel();
            }
        }

        private void HandlePauseInput()
        {
            if (pauseEndScreenController.IsShown)
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
            foreach (var deathTrigger in currentLevelInstance.DeathTriggers)
            {
                deathTrigger.Initialize();
            }
            foreach (var cannon in currentLevelInstance.Cannons)
            {
                cannon.Initialize(cameraController);
            }
            playerInstance.Initialize();
            playerInstance.MoveTo(currentLevelInstance.StartPositon.position);
            playerInstance.Show();
            cameraController.SetCannonMode(false);
            cameraController.MoveToTarget(true);
            Time.timeScale = 1f;
        }
        
        private void InitializeLevel()
        {
            int currentLevelIndex = DataService.Instance.GetCurrentLevel();
            currentLevelInstance = Instantiate(levelCollection.Levels[currentLevelIndex].gameObject, Vector3.zero, Quaternion.identity, levelParent).GetComponent<LevelDescriptor>();
            currentLevelInstance.Goal.OnGoalReached += OnGoalReached;
            foreach (var deathTrigger in currentLevelInstance.DeathTriggers)
            {
                deathTrigger.OnDeathTriggerTouched += OnDeathTriggerTouched;
            }
        }
        
        private void CleanUpLevel()
        {
            if (currentLevelInstance == null) return;
            currentLevelInstance.Goal.OnGoalReached -= OnGoalReached;
            foreach (var deathTrigger in currentLevelInstance.DeathTriggers)
            {
                deathTrigger.OnDeathTriggerTouched -= OnDeathTriggerTouched;
            }
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
            Time.timeScale = 0f;
            DataService.Instance.SetLevelProgress(DataService.Instance.GetCurrentLevel(), true);
            pauseEndScreenController.Show(PauseEndScreenController.PauseEndScreenType.LevelComplete);
        }

        private void OnDeathTriggerTouched()
        {
            playerInstance.Hide();
            Time.timeScale = 0f;
            pauseEndScreenController.Show(PauseEndScreenController.PauseEndScreenType.GameOver);
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
            DataService.Instance.SetCurrentLevel(DataService.Instance.GetCurrentLevel() + 1);
            pauseEndScreenController.Hide();
            InitializeLevel();
            StartLevel();
        }

        private void OnQuitToMenu()
        {
            CleanUpLevel();
            SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
