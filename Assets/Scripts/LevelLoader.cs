namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelLoader : Singleton<LevelLoader>
    {

        public void PauseTheGame(bool setPause)
        {
            if (setPause)
            {

                Time.timeScale = 0;
            }
            else
            {

                Time.timeScale = 1;
            }
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        public void LoadMainLevel()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadSwitchTestLevel()
        {
            SceneManager.LoadScene(2);
        }

        public void LoadCombatTestLevel()
        {
            SceneManager.LoadScene(3);
        }
        

        public void QuitGame()
        {
            Debug.Log("Game Quitted !");
            Application.Quit();
        }

    }
}
