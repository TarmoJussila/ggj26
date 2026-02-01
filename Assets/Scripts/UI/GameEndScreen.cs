using System.Collections;
using Logbound.Gameplay;
using UnityEngine;

namespace Logbound.UI
{
    public class GameEndScreen : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

        private void Awake()
        {
            GameLoseController.OnGameEnded += OnGameEnded;
        }

        private void OnDestroy()
        {
            GameLoseController.OnGameEnded -= OnGameEnded;
        }

        private void OnGameEnded(float obj)
        {
            _scoreText.text = "You survived " + obj + " seconds";

            StartCoroutine(AnimateAlpha());
        }

        private IEnumerator AnimateAlpha()
        {
            CanvasGroup group = GetComponent<CanvasGroup>();

            float t = 0.0f;

            while (t < 1.0f)
            {
                t += Time.deltaTime / 2;

                group.alpha = t;
                yield return null;
            }
        }
    }
}
