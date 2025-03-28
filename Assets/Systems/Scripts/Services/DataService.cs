using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using RoundBallGame.Systems.Data;
using UnityEngine;

namespace RoundBallGame.Systems.Services
{
    public class DataService : MonoBehaviour
    {
        // Singleton pattern
        public static DataService Instance { get; private set; }
        
        [Header("Level Data")]
        [SerializeField] private LevelCollectionSO levelCollection;
        
        private ProgressData ProgressData = new ProgressData();
        private string saveFilePath;
        private FileStream fileStream;
        // Used for going from the menu to the game scene or when hitting the Next Level button
        private int currentLevelIndex = 0;
        
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
        
        private void Initialize()
        {
            saveFilePath = Application.persistentDataPath + "/save.maru";
            LoadProgressData();
            if (ProgressData.LevelsProgressData == null || ProgressData.LevelsProgressData.Length == 0)
            {
                InitializeProgressData();
                SaveProgressData();
                Debug.Log("Fresh progress data created.");
            }
        }

        private void InitializeProgressData()
        {
            ProgressData.LevelsProgressData = new LevelProgressData[levelCollection.Levels.Length];
            for (int i = 0; i < levelCollection.Levels.Length; i++)
            {
                ProgressData.LevelsProgressData[i] = new LevelProgressData
                {
                    LevelIndex = i,
                    IsCompleted = false,
                    CollectibleProgress = new bool[3]
                };
            }
        }
        
        public void SetCurrentLevel(int levelIndex)
        {
            currentLevelIndex = levelIndex;
        }
        
        public int GetCurrentLevel()
        {
            return currentLevelIndex;
        }
        
        public void SetLevelProgress(int levelIndex, bool isCompleted, bool[] collectibleProgress)
        {
            ProgressData.LevelsProgressData[levelIndex].IsCompleted = isCompleted;
            ProgressData.LevelsProgressData[levelIndex].CollectibleProgress = collectibleProgress;
            SaveProgressData();
        }
        
        public LevelProgressData GetLevelProgress(int levelIndex)
        {
            return ProgressData.LevelsProgressData[levelIndex];
        }

        public bool NextLevelExists()
        {
            return currentLevelIndex + 1 < levelCollection.Levels.Length;
        }
        
        public LevelCollectionSO GetLevelCollection()
        {
            return levelCollection;
        }
        
        public void SaveProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();   
            
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Truncate);
                formatter.Serialize(fileStream, ProgressData);
                fileStream.Close();
            }
            else
            {
                fileStream = new FileStream(saveFilePath, FileMode.CreateNew);
                formatter.Serialize(fileStream, ProgressData);
                fileStream.Close();
            }
        }
        
        public void LoadProgressData()
        {
            BinaryFormatter formatter = new BinaryFormatter();        
            if(File.Exists(saveFilePath))
            {
                fileStream = new FileStream(saveFilePath, FileMode.Open);
                ProgressData = formatter.Deserialize(fileStream) as ProgressData;
                fileStream.Close();
            }
            else
            {
                Debug.Log("Progress data not found.");
            }
        }
        
        public void DeleteProgressData()
        {
            if(File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
                Debug.Log("Progress data deleted.");
            }
        }
        
#if UNITY_EDITOR
        [ContextMenu("Delete Progress Data")]
        void ContextMenuDeleteProgressData()
        {
            saveFilePath = Application.persistentDataPath + "/save.maru";
            DeleteProgressData();
        }
#endif
    }
}
