namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Component attached to gameobject to make its collider trigger Interactable on shared TriggerTypes
    /// </summary>
    public class TriggererEvents : MonoBehaviour
    {
        [SerializeField]
        private List<TriggerType> triggererTypes;

        public List<TriggerType> TriggererTypes { get => triggererTypes; }
    }
}
