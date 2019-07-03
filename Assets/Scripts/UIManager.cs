namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class UIManager : Singleton<UIManager>
    {

        [SerializeField]
        private bool showMainMenuCanvasOnStart = false;

        [SerializeField]
        private GameObject mainMenuCanvas;
        
        [SerializeField]
        private GameObject pauseCanvas;

        private bool pauseIsEnabled = false;

        private void Start()
        {
            mainMenuCanvas.SetActive(showMainMenuCanvasOnStart);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel"))
            {
                PauseTheGame(!pauseIsEnabled);
            }
        }

        public void PauseTheGame(bool setPause)
        {
            if (showMainMenuCanvasOnStart) return; //no pause in main menu

            if (setPause)
            {
                pauseCanvas.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pauseCanvas.SetActive(false);
                Time.timeScale = 1;
            }

            pauseIsEnabled = !pauseIsEnabled;
        }
        

        public void LoadMainMenu()
        {
            PauseTheGame(false);
            SceneManager.LoadScene(0);
        }
        public void LoadMainLevel()
        {
            PauseTheGame(false);
            SceneManager.LoadScene(1);
        }

        public void LoadSwitchTestLevel()
        {
            PauseTheGame(false);
            SceneManager.LoadScene(2);
        }

        public void LoadCombatTestLevel()
        {
            PauseTheGame(false);
            SceneManager.LoadScene(3);
        }


        public void QuitGame()
        {
            Debug.Log("Game Quitted !");
            Application.Quit();
        }
    }
}
