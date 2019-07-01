namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LightController : MonoBehaviour
    {

        private MeshRenderer lightRenderer;

        [SerializeField][ReadOnly]
        private bool isOn = false;
        private void Awake()
        {
            lightRenderer = GetComponent<MeshRenderer>();
            isOn = lightRenderer.enabled;
        }
        

        public void SwitchOnOff()
        {
            //reverse
            isOn = !isOn;
            //switch light
            lightRenderer.enabled = isOn;
            
        }
    }

}
