namespace WGJ.PuppetShadow
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerCharacter : Character
    {

        [SerializeField]
        private GunController gunController;

        private Vector2 direction;

        protected override void Awake()
        {
            base.Awake();
            if (gunController == null)
            {
                gunController = GetComponentInChildren<GunController>();
            }
        }

        // Update is called once per frame
        void FixedUpdate()
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
