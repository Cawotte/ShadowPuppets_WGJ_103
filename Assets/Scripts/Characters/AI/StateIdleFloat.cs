namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateIdleFloat : State
    {
        public override void StartState()
        {
        }

        public override void Update()
        {
            //Float around endlessly

            if (!stateMachine.Puppet.IsMoving)
            {
                stateMachine.Puppet.StartIdlingFloat(30f);
            }
        }


        public override void EndState()
        {
            stateMachine.Puppet.StopMovement();
        }
    }
}
