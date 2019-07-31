namespace WGJ.PuppetShadow
{
    using UnityEngine;

    /// <summary>
    /// StateMachine that manage a State and holds shared data for states. 
    /// It's "Context" from the design pattern State.
    /// <para></para>
    /// It's used to control the IA of the puppets. 
    /// </summary>
    public class StateMachine
    {

        private State currentState;
        private Puppet puppet; //the puppet that state machine manage
        private Transform target; //Where the puppet want to go.

        /// <summary>
        /// Initialize the State Machine with a starting State and the puppet it controls.
        /// </summary>
        /// <param name="startingState"></param>
        /// <param name="puppet"></param>
        public StateMachine(State startingState, Puppet puppet) {
            this.target = LevelManager.Instance.GetTarget(); //usually the player, but is a dummmy target in the main menu
            this.puppet = puppet;
            this.CurrentState = startingState;
        }

        /// <summary>
        /// The Current State of the StateMachine, used to change State.
        /// </summary>
        public State CurrentState {
            get => currentState;
            set
            {
                currentState?.EndState(); //end the previous state
                currentState = value;
                currentState.StateMachine = this;
                currentState.StartState(); //initialize the new one
            }
        }

        public Puppet Puppet { get => puppet;  }
        public Transform Target { get => target;  }

        public void Update()
        {
            //Apply current state's behaviour.
            CurrentState.Update();
        }

        public float GetDistanceWithPlayer()
        {
            return Vector3.Distance(puppet.transform.position, target.transform.position);
        }
    }

}
