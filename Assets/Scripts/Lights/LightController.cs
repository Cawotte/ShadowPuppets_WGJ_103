namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LightController : MonoBehaviour
    {
        [SerializeField]
        protected MeshRenderer lightRenderer;

        protected AudioSourcePlayer audioPlayer;

        [SerializeField]
        [ReadOnly]
        protected bool isFlicker = false;

        [SerializeField]
        [ReadOnly]
        private bool isOn = false;

        [SerializeField]
        private bool forceSwitchOn = false;

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
            if (isOn != forceSwitchOn)
            {
                SwitchOnOff(forceSwitchOn);
                isOn = forceSwitchOn;
            }
        }

        public virtual void SwitchOnOff()
        {
            SwitchOnOff(!isOn);
            IsOn = !isOn;
        }

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

        protected void PlaySoundLight()
        {
            StopSoundLight();
            
            if (isFlicker)
            {
                audioPlayer.PlayRandomFromList("lightLoop");
            }
            else
            {
                PickRandomSound();
                audioPlayer.PlaySound(
                    "lightBoot" + soundIndex,
                    () => audioPlayer.PlaySound("lightLoop" + soundIndex));
            }
        }

        private int PickRandomSound()
        {
            soundIndex = UnityEngine.Random.Range(1, 3);
            return soundIndex;
        }
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
