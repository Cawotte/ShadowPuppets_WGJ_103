﻿namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class UIManager : Singleton<UIManager>
    {

        [SerializeField]
        private bool showMainMenuCanvasOnStart = false;

        [SerializeField]
        private GameObject mainMenuCanvas;
        
        [SerializeField]
        private GameObject pauseCanvas;


        [SerializeField]
        private Slider luminositySlider;
        [SerializeField]
        private AmbientLight ambientLight;


        [SerializeField]
        private GameObject deathCanvas;
        [SerializeField]
        private TextMeshProUGUI textDeath;


        [SerializeField]
        private GameObject playerLifeGameobject;
        [SerializeField]
        private TextMeshProUGUI playerLife;


        private bool pauseIsEnabled = false;

        public bool PauseIsEnabled { get => pauseIsEnabled; set => pauseIsEnabled = value; }

        private void Start()
        {
            mainMenuCanvas.SetActive(showMainMenuCanvasOnStart);
            deathCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            luminositySlider.value = LuminosityManager.Instance.Luminosity;
            ambientLight.SetLuminosity(LuminosityManager.Instance.Luminosity);
            
            playerLifeGameobject.SetActive(LevelManager.Instance.Player != null);
        }

        private void Update()
        {
            if (Input.GetButtonDown("Cancel") && !deathCanvas.activeInHierarchy)
            {
                PauseTheGame(!pauseIsEnabled);
            }
        }

        public void SetPlayerLife(int life)
        {
            string lifestr = "";
            for (int i = 0; i < life; i++)
            {
                lifestr += "I";
            }
            playerLife.text = lifestr;
        }
        public void SetLuminosity(float alpha)
        {
            LuminosityManager.Instance.Luminosity = alpha;
            ambientLight.SetLuminosity(alpha);
            luminositySlider.value = alpha;
        }

        public void OpenDeathPanel()
        {
            textDeath.text = "You've defeated " + LevelManager.Instance.Score + " puppets.";
            deathCanvas.SetActive(true);
            PauseTheGame(true);
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
