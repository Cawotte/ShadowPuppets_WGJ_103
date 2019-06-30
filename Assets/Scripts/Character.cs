namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Character : MonoBehaviour
    {

        private Rigidbody2D rb;

        [SerializeField]
        protected float speed = 2.5f;
        [SerializeField]
        protected float jumpStrenght = 6f;


        [Range(0, .3f)]
        [SerializeField]
        private float movementSmoothing = .13f; // How much to smooth out the movement


        private const float speedMultiplier = 100f;
        private const float jumpForceMultiplier = 100f;

        private Vector3 velocity = Vector3.zero;
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }


        protected void MoveToward(Vector2 direction)
        {
            Vector3 movement = direction * Time.fixedDeltaTime * speed;
            rb.MovePosition(transform.position + movement);
        }


        protected void MoveHorizontallyToward(float horizontalMovement)
        {


            float movement = horizontalMovement * Time.fixedDeltaTime * speed * speedMultiplier;
            
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(movement, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
        }

        protected void Jump()
        {
            rb.AddForce(Vector2.up * jumpStrenght * jumpForceMultiplier);
        }
    }

}
