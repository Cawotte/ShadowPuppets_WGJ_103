﻿namespace WGJ.PuppetShadow
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerCharacter : Character
    {

        [Header("Player Character")]

        [SerializeField]
        protected float maxJumpStrenght = 6f;
        [SerializeField]
        protected float minJumpStrenght = 2f;

        [Range(0, .3f)]
        [SerializeField]
        private float movementSmoothing = .13f; // How much to smooth out the movement

        [SerializeField]
        private GunController gunController;

        private Vector2 direction;

        private float timeSinceJumpPress = 0f;
        private float currentJumpStrenght;

        private int jumpDone = 0;


        private const float speedMultiplier = 10f;
        private const float jumpForceMultiplier = 100f;

        protected override void Awake()
        {
            base.Awake();
            if (gunController == null)
            {
                gunController = GetComponentInChildren<GunController>();
                gunController.Player = this;
            }
            currentJumpStrenght = minJumpStrenght;
            transform.rotation *= Quaternion.Euler(0, 180, 0);
            OnDeath += UIManager.Instance.OpenDeathPanel;
        }

        // Update is called once per frame
        void Update()
        {

            HorizontalMovement();

            if (isOnGround && jumpDone >= 1)
            {
                jumpDone = 0;
            }

            JumpMovement();

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

        private void HorizontalMovement()
        {

            direction = GetHorizontalDirectionFromAxis();
            if (direction != Vector2.zero)
            {
                MoveHorizontallyToward(direction.x);

            }
            else
            {
                StopHorizontalMovement();
            }
        }

        private void JumpMovement()
        {
            if (jumpDone >= 1) return;

            float timeForFullJump = 0.08f;
            if (Input.GetButtonDown("Jump"))
            {
                timeSinceJumpPress = 0f;
                currentJumpStrenght = minJumpStrenght;
            }
            else if (Input.GetButton("Jump") && timeSinceJumpPress < timeForFullJump)
            {
                currentJumpStrenght = Mathf.Lerp(minJumpStrenght, maxJumpStrenght, timeSinceJumpPress / timeForFullJump);
                timeSinceJumpPress += Time.deltaTime;
            }
            else if (Input.GetButtonUp("Jump") || timeSinceJumpPress > timeForFullJump)
            {
                if (timeSinceJumpPress > timeForFullJump)
                {
                    currentJumpStrenght = maxJumpStrenght;
                }

                Jump();
                jumpDone++;
                currentJumpStrenght = minJumpStrenght;
                timeSinceJumpPress = timeForFullJump;

            }
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


            float movement = horizontalMovement * Time.deltaTime * speed * speedMultiplier;
            Vector3 veloc = velocity;
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(movement, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            //rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref veloc, movementSmoothing);
            rb.velocity = targetVelocity;
        }

        private void StopHorizontalMovement() {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        protected void Jump()
        {
            rb.AddForce(Vector2.up * currentJumpStrenght, ForceMode2D.Impulse);
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
