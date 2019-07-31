namespace WGJ.PuppetShadow
{

    /// <summary>
    /// State that makes the puppet floats around.
    /// </summary>
    public class StateIdleFloat : State
    {

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
            //On end of state, stop the movement of the puppet to avoid further conflicts.
            stateMachine.Puppet.StopMovement();
        }
    }
}
