namespace WGJ.PuppetShadow
{
    using UnityEngine;

    /// <summary>
    /// Light that's are enabled by a switch and stay On for a limited time.
    /// </summary>
    public class LightTimer : LightController
    {
        [Header("Timer Light")]

        [SerializeField]
        [Range(2, 15)]
        private float durationOn = 5f; //duration the light will stay on.

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
                    //Turn off when the timer is over.
                    SwitchOnOff(false);
                    IsOn = false;
                    StopSoundLight();
                }
                timer += Time.deltaTime;
            }
        }


        public override void SwitchOnOff()
        {
            //Always switch on the light when the button is clicked on.
            SwitchOnOff(true);
            IsOn = true;
            timer = 0f; //reset timer
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
