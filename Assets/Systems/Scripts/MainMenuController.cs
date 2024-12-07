using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RoundBallGame.Systems
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button exitButton;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private SceneAsset levelScene;
        
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
            DataService.Instance.CurrentLevelIndex = 0;
            SceneManager.LoadSceneAsync(levelScene.name, LoadSceneMode.Single);
        }
        
        private void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
