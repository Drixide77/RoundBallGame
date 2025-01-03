using System.Collections;
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
        [Header("Assets")]
        [SerializeField] private string mainMenuSceneName;
        
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
            StartCoroutine(PresentLogosCoroutine());
        }

        private IEnumerator PresentLogosCoroutine()
        {
            foreach (var logo in logosToShow)
            {
                Color temp;
                yield return new WaitForSeconds(initialDelay);
                while (logo.color.a < 1f)
                {
                    temp = logo.color;
                    temp.a += alphaStepAmount;
                    logo.color = temp;
                    yield return new WaitForSeconds(stepTime);
                }
                yield return new WaitForSeconds(holdTime);
                while (logo.color.a > 0f)
                {
                    temp = logo.color;
                    temp.a -= alphaStepAmount;
                    logo.color = temp;
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
