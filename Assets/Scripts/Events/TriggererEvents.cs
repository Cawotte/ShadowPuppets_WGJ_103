namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Like a collision layer, but for trigger Interactables.
    /// </summary>
    public class TriggererEvents : MonoBehaviour
    {
        [SerializeField]
        private List<TriggerType> triggererTypes;

        public List<TriggerType> TriggererTypes { get => triggererTypes; }
    }
}
