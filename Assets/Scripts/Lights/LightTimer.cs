namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LightTimer : LightController
    {
        [Header("Timer Light")]

        [SerializeField]
        [Range(2, 15)]
        private float durationOn = 5f;

        [SerializeField]
        [ReadOnly]
        private float timer = 0f;

        // Update is called once per frame
        void Update()
        {
            if (IsOn)
            {
                if (timer > durationOn)
                {
                    SwitchOnOff(false);
                    IsOn = false;
                    StopSoundLight();
                }
                timer += Time.deltaTime;
            }
        }

        public override void SwitchOnOff()
        {
            SwitchOnOff(true);
            IsOn = true;
            timer = 0f;
        }

        public override void SwitchOnOff(bool on)
        {
            lightRenderer.enabled = on;
            if (on && !IsOn)
            {
                PlaySoundLight();
            }
        }

    }
}
