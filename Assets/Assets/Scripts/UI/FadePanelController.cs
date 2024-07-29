
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UI {
    public class FadePanelController : MonoBehaviour {
        private Image image;

        public void Awake() {
            image = GetComponent<Image>();
        }

        public void FadeOut(Action onComplete, float fadeTime = 1, float delay = 0) {
            StartCoroutine(FadeCoroutine(1, 0, onComplete, fadeTime, delay));
        }

        public void FadeIn(Action onComplete, float fadeTime = 1, float delay = 0) {
            StartCoroutine(FadeCoroutine(0, 1, onComplete, fadeTime, delay));
        }

        public void SetAlpha(float alpha) {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private IEnumerator FadeCoroutine(int startAlpha, int targetAlpha, Action onComplete, float fadeTime, float delay) {
            yield return new WaitForSeconds(delay);

            // Set initial alpha and set active
            SetAlpha(startAlpha);

            // Calculate increments
            float tick = Time.deltaTime;
            int increments = (int)(1/tick * fadeTime);
            float step = (targetAlpha - startAlpha) / (float)increments;

            for (int i = 0; i < increments; i++) {
                SetAlpha(image.color.a + step);
                yield return new WaitForSeconds(tick);
            }

            onComplete?.Invoke();
        }
    }
}