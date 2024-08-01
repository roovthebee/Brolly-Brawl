
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class MainMenu : MonoBehaviour {
        private GameObject primaryPanel;
        private GameObject levelMenu;
        private GameObject currentLevel;
        private GameObject controlsMenu;

        private void Start() {
            primaryPanel = transform.Find("Panel")?.gameObject;
            levelMenu = primaryPanel.transform.Find("LevelMenu")?.gameObject;
            currentLevel = levelMenu.transform.Find("Level 1")?.gameObject;
            controlsMenu = primaryPanel.transform.Find("ControlsMenu")?.gameObject;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                levelMenu.SetActive(false);
                controlsMenu.SetActive(false);
            }
        }

        public void OnPlayClicked() {
            SceneManager.LoadScene(currentLevel.name);
        }

        public void OnLevelClicked() {
            if (!levelMenu.activeSelf) {
                levelMenu.SetActive(true);
            }
        }

        public void OnLevelBackClicked() {
            if (levelMenu.activeSelf) {
                levelMenu.SetActive(false);
            }
        }

        public void OnLevelSelected(string buttonName) {
            if (buttonName != currentLevel.name) {
                currentLevel.GetComponent<Button>().interactable = true;
                currentLevel = levelMenu.transform.Find(buttonName)?.gameObject;
                currentLevel.GetComponent<Button>().interactable = false;
            }
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

        public void OnExitGame() {
            #if UNITY_STANDALONE
                Application.Quit();
            #endif

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}