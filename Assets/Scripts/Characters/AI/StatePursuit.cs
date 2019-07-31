namespace WGJ.PuppetShadow
{
    using UnityEngine;

    /// <summary>
    /// State that contains the IA to make a Shadow Puppet pursue the player.
    /// </summary>
    public class StatePursuit : State
    {
        //Keep track at how much time passed since last time the path to the player position
        private float timePursuit = 0f;

        public override void Update()
        {
            //Go toward the player
            float distance = stateMachine.GetDistanceWithPlayer();
            
            if (!stateMachine.Puppet.IsMoving)
            {
                //If we are not very close to the player, pursue them
                if (distance > 1f)
                {
                    Pursuit();
                }
                else
                {
                    //else float on them.
                    IdleFloat(); //last 1 second
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
                //Every 5 seconds, recalculate a path anyway.
                if (timePursuit > 5f)
                {
                    Pursuit();
                }
            }

            timePursuit += Time.deltaTime;

        }


        public override void EndState()
        {
            //On end of state, stop the movement of the puppet to avoid further conflicts.
            stateMachine.Puppet.StopMovement();
        }

        /// <summary>
        /// Calculate the path and make the puppet go to the player position.
        /// </summary>
        private void Pursuit()
        {
            stateMachine.Puppet.StopMovement();
            stateMachine.Puppet.MoveTo(stateMachine.Target.position);
            timePursuit = 0f; //reset pursuit count
        }

        /// <summary>
        /// Float aimlessly for a second.
        /// </summary>
        private void IdleFloat()
        {
            stateMachine.Puppet.StopMovement();
            stateMachine.Puppet.StartIdlingFloat(1f);
            timePursuit = 0f; //reset pursuit count
        }
    }
}
