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
        
        [Header("Assets")]
        [SerializeField] private SceneAsset levelScene;
        
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
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
