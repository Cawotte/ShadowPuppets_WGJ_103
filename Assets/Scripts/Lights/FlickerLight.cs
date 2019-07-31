namespace WGJ.PuppetShadow
{
    
    using UnityEngine;
    
    public class FlickerLight : LightController
    {
        [Header("Flicker")]

        // Values for flicker with a constant on and off durations.
        [SerializeField]
        private bool randomFlicker = false;
        [Header("Constant Flicker")]
        [SerializeField]
        [Range(0, 8f)]
        private float lightOnDuration;
        [SerializeField]
        [Range(0, 8f)]
        private float lightOffDuration;

        [Header("Random Flicker")]
        //Values for flickers with a random On and Off ranges.
        [SerializeField]
        private Vector2 randomFlickerRangeOn;
        [SerializeField]
        private Vector2 randomFlickerRangeOff;

        private float timer = 0f;
        private float duration = 0f;

        protected override void Awake()
        {
            base.Awake();
            isFlicker = true;
        }

        protected override void Start()
        {
            SwitchOnOff(IsOn);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
            //every so often, switch the light on/off depending on the duration of the current mode.
            if (timer > duration)
            {
                SwitchOnOff();
            }
        }

        public override void SwitchOnOff()
        {
            base.SwitchOnOff();
            duration = PickNewDuration();
            timer = 0f;
        }

        public override void SwitchOnOff(bool on)
        {
            base.SwitchOnOff(on);
            duration = PickNewDuration();
            timer = 0f;
        }

        /// <summary>
        /// Pick a new duration for the current light mode, depending on if it's random or not, and on or off.
        /// </summary>
        /// <returns></returns>
        private float PickNewDuration()
        {
            
            if (randomFlicker)
            {
                if (IsOn)
                    return Random.Range(randomFlickerRangeOff.x, randomFlickerRangeOff.y);
                else
                    return Random.Range(randomFlickerRangeOn.x, randomFlickerRangeOn.y);
            }
            else
            {
                if (IsOn)
                    return lightOffDuration;
                else
                    return lightOnDuration;
            }
        }
    }
}
