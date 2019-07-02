namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class StatePursuit : State
    {
        private float timePursuit = 0f;
        public override void StartState()
        {
        }

        public override void Update()
        {
            //Go toward the player

            float distance = stateMachine.DistanceWithPlayer();

            if (!stateMachine.Puppet.IsMoving)
            {
                if (distance > 1f)
                {
                    Pursuit();
                }
                else
                {
                    IdleFloat();
                }
            }
            else
            {

                //If very close to the player, recalculte more frequently.
                if (distance < 2f && timePursuit > 1f)
                {
                    Pursuit();
                }
                //Else if relatively close
                else if (distance < 5f && timePursuit > 2f)
                {
                    Pursuit();
                }
                //Every 5 seconds, recalculate a path.
                if (timePursuit > 5f)
                {
                    Pursuit();
                }
            }

            timePursuit += Time.deltaTime;

        }


        public override void EndState()
        {
            stateMachine.Puppet.StopMovement();
        }

        private void Pursuit()
        {
            stateMachine.Puppet.StopMovement();
            stateMachine.Puppet.MoveTo(stateMachine.Player.transform.position);
            timePursuit = 0f;
        }

        private void IdleFloat()
        {
            stateMachine.Puppet.StopMovement();
            stateMachine.Puppet.StartIdlingFloat(1f);
            timePursuit = 0f;
        }
    }
}
