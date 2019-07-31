namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.Events;
    using System.Linq;

    /// <summary>
    /// Component to trigger events when colliding with certains marker triggerers (TriggererEvents) 
    /// </summary>
    public class TriggerEvents : MonoBehaviour
    {
        [SerializeField]
        private List<TriggerType> isTriggeredBy = new List<TriggerType>();

        public Action OnEnterTrigger = null;
        public Action OnExitTrigger = null;

        [SerializeField]
        private UnityEvent OnEnterTriggerEvent = null;
        [SerializeField]
        private UnityEvent OnExitTriggerEvent = null;

        private void Start()
        {
            if (GetComponent<Collider2D>() == null)
            {
                Debug.Log("No collider 2D detected with Trigger! Add one ! (" + gameObject.name + ")");
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsTriggerValid(collision))
            {
                OnEnterTriggerEvent?.Invoke();
                OnEnterTrigger?.Invoke();
            }
        }


        private void OnTriggerExit2D(Collider2D collision)
        {

            if (IsTriggerValid(collision))
            {
                OnExitTrigger?.Invoke();
                OnExitTriggerEvent?.Invoke();
            }
        }

        private bool IsTriggerValid(Collider2D coll)
        {
            //check errors
            if (coll == null || coll.gameObject == null) {
                return false;
            }

            TriggererEvents triggerer = coll.GetComponent<TriggererEvents>();

            //Check if has triggerer component
            if (triggerer == null)
            {
                return false;
            }

            //Check if valid triggerer type
            //If there's more than one common triggerer, it's valid.
            return (triggerer.TriggererTypes.Intersect(isTriggeredBy).Count() > 0);

        }
    }

}
