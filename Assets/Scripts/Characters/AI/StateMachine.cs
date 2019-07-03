namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateMachine
    {

        private State currentState;
        private Puppet puppet;
        private Transform target;

        public StateMachine(State startingState, Puppet puppet) {
            this.target = LevelManager.Instance.GetTarget();
            this.puppet = puppet;
            this.CurrentState = startingState;
        }

        public State CurrentState {
            get => currentState;
            set
            {
                currentState?.EndState();
                currentState = value;
                currentState.StateMachine = this;
                currentState.StartState();
            }
        }

        public Puppet Puppet { get => puppet;  }
        public Transform Target { get => target;  }

        public void Update()
        {
            CurrentState.Update();
        }

        public float DistanceWithPlayer()
        {
            return Vector3.Distance(puppet.transform.position, target.transform.position);
        }
    }

}
