namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LightSwitch : Interactable
    {
        [SerializeField]
        private List<LightController> lightsToSwitch = new List<LightController>();

        AudioSourcePlayer sourcePlayer;

        protected override void Awake()
        {
            base.Awake();
            OnTrigger += SwitchOnOffAllLights;
            OnTrigger += PlayClickSound;
            sourcePlayer = gameObject.AddComponent<AudioSourcePlayer>();
        }
        

        private void SwitchOnOffAllLights() {
            for (int i = 0; i < lightsToSwitch.Count; i++)
            {
                lightsToSwitch[i].SwitchOnOff();
            }
        }

        private void PlayClickSound()
        {
            sourcePlayer.PlayRandomFromList("switch");
        }

        private void OnDrawGizmos()
        {
            if (lightsToSwitch != null)
            {
                Gizmos.color = Color.yellow;
                foreach (LightController light in lightsToSwitch)
                {
                    if (light == null) continue;
                    Gizmos.DrawLine(transform.position, light.transform.position);
                }
            }
        }
    }
}
