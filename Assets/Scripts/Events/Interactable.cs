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

        [ReadOnly]
        [SerializeField]
        protected bool isOn = false;
        
        [SerializeField]
        private Sprite otherSprite;

        private SpriteRenderer sr;

        public bool IsOn { get => isOn;}

        protected virtual void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }
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

            if (otherSprite != null)
            {
                Sprite temp = sr.sprite;
                sr.sprite = otherSprite;
                otherSprite = temp;
            }
        }


    }
}
