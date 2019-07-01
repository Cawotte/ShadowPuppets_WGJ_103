namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class Interactable : MonoBehaviour
    {

        public Action OnTrigger = null;
        public Action OnTriggerOn = null;
        public Action OnTriggerOff = null;


        [SerializeField]
        private UnityEvent OnTriggerEvent = null;
        [SerializeField]
        private UnityEvent OnTriggerOnEvent = null;
        [SerializeField]
        private UnityEvent OnTriggerOffEvent = null;

        [SerializeField]
        protected bool isOn = false;

        public bool IsOn { get => isOn;}

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

            isOn = !isOn;
        }


    }
}
