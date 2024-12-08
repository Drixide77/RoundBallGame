using RoundBallGame.Systems.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RoundBallGame.Systems
{
    public class LevelButtonController : MonoBehaviour
    {
        private int levelIndex;
        private LevelProgressData levelProgress;
        
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Button button;
        string levelScene = "SCN_Level";
        
        private void Awake()
        {
            button.onClick.AddListener(OnButtonClicked);
        }
        
        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }
        
        public void SetButtonInfo(int levelIndex, LevelProgressData levelProgress, string levelSceneName)
        {
            this.levelIndex = levelIndex;
            this.levelProgress = levelProgress;
            levelScene = levelSceneName;
            image.color = levelProgress.IsCompleted ? Color.green : Color.white;
            string buttonText = "";
            if (levelIndex < 9) buttonText += "0";
            buttonText += levelIndex + 1;
            text.text = buttonText;
        }

        private void OnButtonClicked()
        {
            DataService.Instance.SetCurrentLevel(levelIndex);
            SceneManager.LoadSceneAsync(levelScene, LoadSceneMode.Single);
        }
    }
}