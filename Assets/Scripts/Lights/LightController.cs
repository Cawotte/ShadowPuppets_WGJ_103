namespace WGJ.PuppetShadow
{
    using UnityEngine;

    /// <summary>
    /// Component to control a light more easily, handle on/off and sounds. 
    /// Is the base class for special lights (timer and flicker)
    /// </summary>
    public class LightController : MonoBehaviour
    {
        [SerializeField]
        protected MeshRenderer lightRenderer; //the light renderer, used to switch on/off the light

        protected AudioSourcePlayer audioPlayer; //The player that play the light sounds.

        [SerializeField]
        [ReadOnly]
        protected bool isFlicker = false; 

        [SerializeField]
        [ReadOnly]
        private bool isOn = false; //The current state of the light

        [SerializeField]
        private bool forceSwitchOn = false; //Change it in the inspector to force change the light, used to change the starting mode

        private int soundIndex = 1;

        protected bool IsOn {
            get => isOn;
            set
            {
                isOn = value;
                forceSwitchOn = value;
            }
        }

        protected virtual void Awake()
        {
            audioPlayer = gameObject.AddComponent<AudioSourcePlayer>();
            isOn = lightRenderer.enabled;

            IsOn = forceSwitchOn;
        }

        protected virtual void Start()
        {

            SwitchOnOff(isOn);
        }

        private void LateUpdate()
        {
            //If forceSwitch has been changed, change it.
            if (isOn != forceSwitchOn)
            {
                SwitchOnOff(forceSwitchOn);
                isOn = forceSwitchOn;
            }
        }

        /// <summary>
        /// Switch the light on/off to the opposite of the current value.
        /// </summary>
        public virtual void SwitchOnOff()
        {
            SwitchOnOff(!isOn);
            IsOn = !isOn;
        }

        /// <summary>
        /// Switch the light on/off with the chosen bool.
        /// </summary>
        /// <param name="on"></param>
        public virtual void SwitchOnOff(bool on)
        {
            lightRenderer.enabled = on;
            if (on)
            {
                PlaySoundLight();
            }
            else
            {
                StopSoundLight();
            }
        }

        /// <summary>
        /// Start the light sound.
        /// </summary>
        protected void PlaySoundLight()
        {
            StopSoundLight();
            
            if (isFlicker)
            {
                //For flickering light we don't use a boot sounds because it may end very early, so we just use a long noise.
                audioPlayer.PlayRandomFromList("lightLoop");
            }
            else
            {
                //Pick and play a random matching pair or booting light + looped light sounds.
                PickRandomSound();
                audioPlayer.PlaySound(
                    "lightBoot" + soundIndex,
                    () => audioPlayer.PlaySound("lightLoop" + soundIndex));
            }
        }

        /// <summary>
        /// Pick an index for a random matching booting + light loop pair sounds.
        /// </summary>
        /// <returns></returns>
        private int PickRandomSound()
        {
            soundIndex = UnityEngine.Random.Range(1, 3);
            return soundIndex;
        }

        /// <summary>
        /// Stop the currently played light sounds.
        /// </summary>
        protected void StopSoundLight()
        {

            if (!isFlicker)
            {
                audioPlayer.InterruptSound("lightLoop" + soundIndex);
                audioPlayer.InterruptSound("lightBoot" + soundIndex);
            }
            else
            {
                audioPlayer.InterruptSound("lightLoop");
                audioPlayer.InterruptSound("lightBoot");
            }
        }
    }

}
