namespace WGJ.PuppetShadow
{


    using Light2D;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    public class FlickerLight : LightController
    {
        [Header("Flicker")]

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
            SwitchOnOff(isOn);
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
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

        private float PickNewDuration()
        {
            
            if (randomFlicker)
            {
                if (isOn)
                    return Random.Range(randomFlickerRangeOff.x, randomFlickerRangeOff.y);
                else
                    return Random.Range(randomFlickerRangeOn.x, randomFlickerRangeOn.y);
            }
            else
            {
                if (isOn)
                    return lightOffDuration;
                else
                    return lightOnDuration;
            }
        }
    }
}
