namespace WGJ.PuppetShadow
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Component for Light Switchs. Link them to the lights they interact with.
    /// </summary>
    public class LightSwitch : Interactable
    {
        [SerializeField]
        //List of light they switch on/off
        private List<LightController> lightsToSwitch = new List<LightController>();

        AudioSourcePlayer sourcePlayer;

        protected override void Awake()
        {
            base.Awake();

            //A Light switch is triggered by the player. When it's triggered, 
            // will play a click sound and switch all lights.
            OnTrigger += SwitchOnOffAllLights;
            OnTrigger += PlayClickSound;

            sourcePlayer = gameObject.AddComponent<AudioSourcePlayer>();
        }
        
        /// <summary>
        /// Switch on/off all the lights that switch can interact with.
        /// </summary>
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

        //Draw a line from the switch to all the lights it interacts with. Use Gizmos. In-Editor only.
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
