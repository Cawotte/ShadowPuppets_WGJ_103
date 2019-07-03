namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;


    public class DifficultyManager : MonoBehaviour
    {
        [SerializeField]
        private bool disableSpawner = false;

        [Header("Passive Spawner")]
        [SerializeField]
        private float timeBeforeNewSpawn = 30f; //Time before spawning a new puppets.
        [SerializeField]
        private float minimalSpawnTime = 5f;
        [SerializeField]
        private int minimalPuppets = 0;
        [SerializeField]
        private int maxPuppets = 10;
        [SerializeField]
        [ReadOnly]
        private float timerNewSpawn = 0f;

        [Header("Passive Decrement")]
        [SerializeField]
        private bool passiveDecrement = true;
        [SerializeField]
        private float timeDecreaseSpawnTime = 10f; //Time before decrasing passive time.
        [SerializeField]
        [ReadOnly]
        private float timerDecrease = 0f;

        [Header("Minimal Increment")]
        [SerializeField]
        private bool minimalIncrement = true;
        [SerializeField]
        private float timerMinimalIncrement = 10f;
        [SerializeField]
        [ReadOnly]
        private float timerMin = 0f;


        [Header("Maximal Increment")]
        [SerializeField]
        private bool maxIncrement = true;
        [SerializeField]
        private float timerMaxIncrement = 10f;
        [SerializeField]
        [ReadOnly]
        private float timerMax = 0f;

        private int PuppetsOnHold = 0; //MaxCount was reached at end of timer, so we wait for max room.

        private LevelManager levelManager
        {
            get => LevelManager.Instance;
        }

        private int Score
        {
            get => levelManager.Score;
        }

        private int PuppetCount
        {
            get => levelManager.CurrentPuppets;
        }
        
        // Update is called once per frame
        void Update()
        {
            if (disableSpawner) return;

            HandlePassiveSpawn();
            HandlePassiveDecrement();
            HandleMinimalPuppets();
            HandleMaxPuppets();
        }

        private void HandlePassiveSpawn()
        {

            if (timerNewSpawn > timeBeforeNewSpawn)
            {
                //If room for new puppets
                if (PuppetCount < maxPuppets)
                {
                    levelManager.SpawnNewPuppet();
                }
                else
                {
                    //save it for later
                    PuppetsOnHold++;
                }

                timerNewSpawn = 0f;

            }
            else
            {
                timerNewSpawn += Time.deltaTime;
            }

        }

        private void HandlePassiveDecrement()
        {
            if (!passiveDecrement || minimalSpawnTime == timeBeforeNewSpawn)
                return;

            if (timerDecrease > timeDecreaseSpawnTime)
            {
                //Decrease will respecting min bound
                timeBeforeNewSpawn = Mathf.Max(minimalSpawnTime, timeBeforeNewSpawn - 1f);

                timerDecrease = 0f;
            }
            else
            {
                timerDecrease += Time.deltaTime;
            }
        }

        private void HandleMinimalPuppets()
        {
            if (PuppetCount < minimalPuppets)
            {
                levelManager.SpawnNewPuppet();
            }

            //Difficulty ramp
            if (minimalIncrement)
            {
                //Increase minimal puppet count gradually over time
                if (timerMin > timerMinimalIncrement)
                {
                    minimalPuppets++;
                    timerMin = 0f;
                }
                else
                {
                    timerMin += Time.deltaTime;
                }

            }
        }

        private void HandleMaxPuppets()
        {
            //Difficulty ramp
            if (maxIncrement)
            {
                //Increase maximal puppet count gradually over time
                if (timerMax > timerMaxIncrement)
                {
                    maxPuppets++;
                    timerMax = 0f;
                }
                else
                {
                    timerMax += Time.deltaTime;
                }

            }

            //If there's room for a new puppet, spawn it
            if (PuppetsOnHold > 0 && PuppetCount < maxPuppets)
            {
                levelManager.SpawnNewPuppet();
                PuppetsOnHold--;
            }
        }
    }
}