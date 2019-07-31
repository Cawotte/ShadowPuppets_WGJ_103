namespace WGJ.PuppetShadow
{

    /// <summary>
    /// State that implements the IA for the Original Puppets. Do nothing, then
    /// switch to Float state when the player get close. 
    /// </summary>
    public class StateHide : State
    {

        public override void Update()
        {
            //Do nothing

            //If the player get close enough. Start moving aimlessly around.
            if (stateMachine.GetDistanceWithPlayer() < 5f)
            {
                stateMachine.CurrentState = new StateIdleFloat();
            }
        }
       
    }
}
