namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TriggererEvents : MonoBehaviour
    {
        [SerializeField]
        private List<TriggerType> triggererTypes;

        public List<TriggerType> TriggererTypes { get => triggererTypes; }
    }
}
