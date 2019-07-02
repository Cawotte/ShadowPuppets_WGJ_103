namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public abstract class State
    {
        protected StateMachine stateMachine;

        public StateMachine StateMachine { get => stateMachine; set => stateMachine = value; }

        public virtual void StartState()
        {
        }

        public abstract void Update();


        public virtual void EndState()
        {
        }
    }

}
