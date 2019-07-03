namespace WGJ.PuppetShadow
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerCharacter : Character
    {

        [Header("Player Character")]

        [SerializeField]
        protected float jumpStrenght = 6f;

        [Range(0, .3f)]
        [SerializeField]
        private float movementSmoothing = .13f; // How much to smooth out the movement

        [SerializeField]
        private GunController gunController;

        private Vector2 direction;

        private const float speedMultiplier = 100f;
        private const float jumpForceMultiplier = 100f;

        protected override void Awake()
        {
            base.Awake();
            if (gunController == null)
            {
                gunController = GetComponentInChildren<GunController>();
                gunController.Player = this;
            }
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            OnDeath += UIManager.Instance.OpenDeathPanel;
        }

        // Update is called once per frame
        void Update()
        {
            direction = GetHorizontalDirectionFromAxis();
            if (direction != Vector2.zero)
            {
                MoveHorizontallyToward(direction.x);

            }

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetButtonDown("Fire2"))
            {
                InteractWithNearInteractables();
            }

            if (isMoving && isOnGround)
            {
                if (!soundPlayer.IsCurrentlyPlayed("footstep"))
                {
                    soundPlayer.PlayRandomFromList("footstep");
                }
            }

            FacePos(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        }


        protected void InteractWithNearInteractables()
        {
            Collider2D[] colls = new Collider2D[20];
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = true;
            GetComponent<Collider2D>().OverlapCollider(contactFilter, colls);

            foreach (Collider2D coll in colls)
            {
                coll?.GetComponent<Interactable>()?.SwitchOnOff();
            }
        }


        protected void MoveHorizontallyToward(float horizontalMovement)
        {


            float movement = horizontalMovement * Time.fixedDeltaTime * speed * speedMultiplier;
            Vector3 veloc = velocity;
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(movement, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref veloc, movementSmoothing);
        }

        protected void Jump()
        {
            rb.AddForce(Vector2.up * jumpStrenght * jumpForceMultiplier);
        }

        private Vector2 GetDirectionFromAxis()
        {
            return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        }

        private Vector2 GetHorizontalDirectionFromAxis()
        {
            return new Vector2(Input.GetAxis("Horizontal"), 0).normalized;
        }

        
    }
}
