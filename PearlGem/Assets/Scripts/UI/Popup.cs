using System.Collections;
using UnityEngine;

namespace PearlGem.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Popup : MonoBehaviour
    {
        [SerializeField] float fadeDuration = 0.5f;

        CanvasGroup _canvasGroup;

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(FadeCanvasGroup(0f, 1f, fadeDuration));
        }

        public void Hide()
        {
            StopAllCoroutines();
            StartCoroutine(FadeOutAndDisable(fadeDuration));
        }

        IEnumerator FadeCanvasGroup(float start, float end, float duration)
        {
            float elapsed = 0f;
            _canvasGroup.alpha = start;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }

            _canvasGroup.alpha = end;
        }

        IEnumerator FadeOutAndDisable(float duration)
        {
            yield return FadeCanvasGroupRoutine(1f, 0f, duration);
            gameObject.SetActive(false);
        }
    
        IEnumerator FadeCanvasGroupRoutine(float start, float end, float duration)
        {
            float elapsed = 0f;
            _canvasGroup.alpha = start;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
                yield return null;
            }

            _canvasGroup.alpha = end;
        }
    }
}
