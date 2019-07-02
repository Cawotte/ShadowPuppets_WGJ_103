namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StateHide : State
    {
        public override void StartState()
        {
        }

        public override void Update()
        {
            //Do nothing

            //If the player get close enough. Start moving aimlessly around.
            if (stateMachine.DistanceWithPlayer() < 5f)
            {
                stateMachine.CurrentState = new StateIdleFloat();
            }
        }


        public override void EndState()
        {
        }
    }
}
