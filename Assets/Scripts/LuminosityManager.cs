namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LuminosityManager : Singleton<LuminosityManager>
    {

        [Range(0f, 255f)]
        [SerializeField]
        private float luminosity = 70f;

        public float Luminosity { get => luminosity;
            set
            {
                luminosity = value;
            }
        }
    }

}
