namespace WGJ.PuppetShadow
{
    using System;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Component and base class that holds events that can be called and triggered by other scripts. 
    /// Usually through collisions.
    /// </summary>
    public class Interactable : MonoBehaviour
    {

        //Action can only be defined in code.

        public Action OnTrigger = null; //Action to perform at each switch.
        public Action OnTriggerOn = null; //Action to perform on switch on.
        public Action OnTriggerOff = null; //Action to perform on switch off.

        //UnityEvent are mainly defined in the inspector.
        [SerializeField]
        private UnityEvent OnTriggerEvent = null; //Event to perform at each switch.
        [SerializeField]
        private UnityEvent OnTriggerOnEvent = null; //Event to perform on switch on.
        [SerializeField]
        private UnityEvent OnTriggerOffEvent = null; //Event to perform on switch off.

        [ReadOnly]
        [SerializeField]
        protected bool isOn = false; //keep track of on/off

        [SerializeField]
        private Sprite otherSprite; //A sprite to swap with the current gameobject sprite when switched on/off

        private SpriteRenderer sr;

        public bool IsOn { get => isOn; }

        protected virtual void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Switch the Interactable "on/off" and trigger the matching events. 
        /// </summary>
        public void SwitchOnOff()
        {
            OnTriggerEvent?.Invoke();
            OnTrigger?.Invoke();

            //switch off
            if (isOn)
            {
                OnTriggerOffEvent?.Invoke();
                OnTriggerOff?.Invoke();
            }
            //switch on
            else
            {
                OnTriggerOnEvent?.Invoke();
                OnTriggerOn?.Invoke();
            }

            //new mode
            isOn = !isOn;

            //switch sprites
            if (otherSprite != null)
            {
                Sprite temp = sr.sprite;
                sr.sprite = otherSprite;
                otherSprite = temp;
            }
        }


    }
}
