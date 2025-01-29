using System;
using System.Collections;
using RoundBallGame.Systems.Data;
using RoundBallGame.Systems.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoundBallGame.Systems
{
    public class IntroSceneController : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private SpriteRenderer[] logosToShow;
        [Space(10)]
        [Header("Settings")]
        [SerializeField] private float initialDelay;
        [SerializeField] private float alphaStepAmount;
        [SerializeField] private float stepTime;
        [SerializeField] private float holdTime;
        [SerializeField] private float endSequenceDelay;
        [Space(10)]
        [Header("Audio")]
        [SerializeField] private AudioRepositoryEntryId[] audiosToPlay;
        [Space(10)]
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;

        private Coroutine animationCoroutine;
        
        private void Awake()
        {
            Color temp;
            foreach (var logo in logosToShow)
            {
                temp = logo.color;
                temp.a = 0f;
                logo.color = temp;
            }
        }

        private void Start()
        {
            animationCoroutine = StartCoroutine(PresentLogosCoroutine());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
            {
                LoadScene();
            }
        }

        private void OnDestroy()
        {
            if (animationCoroutine != null) StopCoroutine(animationCoroutine);
        }

        private IEnumerator PresentLogosCoroutine()
        {
            for (int i = 0; i < logosToShow.Length; ++i)
            {
                Color temp;
                yield return new WaitForSeconds(initialDelay);
                AudioService.Instance.PlayMusicClip(audiosToPlay[i]);
                while (logosToShow[i].color.a < 1f)
                {
                    temp = logosToShow[i].color;
                    temp.a += alphaStepAmount;
                    logosToShow[i].color = temp;
                    yield return new WaitForSeconds(stepTime);
                }
                yield return new WaitForSeconds(holdTime);
                while (logosToShow[i].color.a > 0f)
                {
                    temp = logosToShow[i].color;
                    temp.a -= alphaStepAmount;
                    logosToShow[i].color = temp;
                    yield return new WaitForSeconds(stepTime);
                }
            }
            yield return new WaitForSeconds(endSequenceDelay);

            LoadScene();
        }

        private void LoadScene()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneName, LoadSceneMode.Single);
        }
    }
}
