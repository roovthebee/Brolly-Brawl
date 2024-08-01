
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace UI {
    public class GameInterfaceController : MonoBehaviour {
        public GameObject fadePanel;
        public GameObject pauseMenu;
        private GameObject pauseButton;
        public GameObject dash;
        private GameObject timer;
        private GameObject controlsMenu;
        private float currentTime;

        private void Start() {
            pauseButton = transform.Find("Pause")?.gameObject;
            dash = transform.Find("Dash")?.gameObject;
            timer = transform.Find("Timer")?.gameObject;
            controlsMenu = pauseMenu.transform.Find("ControlsMenu")?.gameObject;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                TogglePauseMenu();
            }

            // Update timer
            float increment = Time.deltaTime;
            currentTime += increment;

            int minutes = (int)Mathf.Floor(currentTime / 60f);
            int seconds = (int)Mathf.Floor(currentTime - (minutes * 60f));
            int milliseconds = (int)Mathf.Floor((currentTime - Mathf.Floor(currentTime)) * 1000);

            string secondsPretext = seconds < 10 ? ":0" : ":";
            string millisecondsPretext = milliseconds >= 10 ? milliseconds >= 100 ? "." : ".0" : ".00";

            timer.GetComponent<TMP_Text>().text = minutes + secondsPretext + seconds + millisecondsPretext + milliseconds;
        }

        private void TogglePauseMenu() {
            if (pauseMenu.activeSelf) {
                Time.timeScale = 1f;
                OnControlsBackClicked();
                pauseButton.SetActive(true);
                pauseMenu.SetActive(false);
            } else {
                Time.timeScale = 0f;
                pauseButton.SetActive(false);
                pauseMenu.SetActive(true);
            }
        }

        public void DashEnable() {
            dash.GetComponent<TMP_Text>().color = Color.white;
        }

        public void DashDisable() {
            dash.GetComponent<TMP_Text>().color = new Color(0.4f, 0.4f, 0.4f);
        }

        public void OnPauseClicked() {
            TogglePauseMenu();
        }

        public void OnResumeClicked() {
            TogglePauseMenu();
        }

        public void OnControlsClicked() {
            if (!controlsMenu.activeSelf) {
                controlsMenu.SetActive(true);
            }
        }

        public void OnControlsBackClicked() {
            if (controlsMenu.activeSelf) {
                controlsMenu.SetActive(false);
            }
        }

        public void OnMainMenuClicked() {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }

        public void OnExitGame() {
            #if UNITY_STANDALONE
                Application.Quit();
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        private void SetAlpha(Image image, float alpha) {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private IEnumerator FadeCoroutine(int startAlpha, int targetAlpha, Action onComplete, float fadeTime, float delay) {
            yield return new WaitForSeconds(delay);

            Image image = fadePanel.GetComponent<Image>();
            SetAlpha(image, startAlpha);

            // Calculate increments
            float tick = Time.deltaTime;
            int increments = (int)(1/tick * fadeTime);
            float step = (targetAlpha - startAlpha) / (float)increments;

            for (int i = 0; i < increments; i++) {
                SetAlpha(image, image.color.a + step);
                yield return new WaitForSeconds(tick);
            }

            onComplete?.Invoke();
        }

        public void FadeScreenOut(Action onComplete, float fadeTime = 1f, float delay = 0f) {
            StartCoroutine(FadeCoroutine(1, 0, onComplete, fadeTime, delay));
        }

        public void FadeScreenIn(Action onComplete, float fadeTime = 1f, float delay = 0f) {
            StartCoroutine(FadeCoroutine(0, 1, onComplete, fadeTime, delay));
        }
    }
}
