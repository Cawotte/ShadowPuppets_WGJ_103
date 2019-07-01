﻿namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class Character : MonoBehaviour
    {

        private Rigidbody2D rb;
        [SerializeField]
        protected int totalLife;
        [SerializeField]
        private int currentLife;

        [SerializeField]
        protected float speed = 2.5f;
        [SerializeField]
        protected float jumpStrenght = 6f;


        [Range(0, .3f)]
        [SerializeField]
        private float movementSmoothing = .13f; // How much to smooth out the movement

        private bool isInvincible = false;

        //Events
        public Action<int> OnDamageTaken = null;
        public Action<int> OnLifeChange = null;
        public Action OnDeath = null;

        private const float speedMultiplier = 100f;
        private const float jumpForceMultiplier = 100f;

        //internals var
        private Vector3 velocity = Vector3.zero;

        #region Properties
        public int CurrentLife {
            get => currentLife;
            protected set
            {
                //no change
                if (value == currentLife) return;

                //clamp
                int newValue = Mathf.Clamp(value, 0, totalLife);
                currentLife = newValue;
                OnLifeChange?.Invoke(newValue);
            }
        }

        #endregion
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            currentLife = totalLife;
        }

        #region Public Methods

        /// <summary>
        /// Deals damage to the character, returns true if it dies.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public bool DealDamage(int damage)
        {
            if (isInvincible)
            {
                return false;
            }

            CurrentLife -= damage;

            if (CurrentLife <= 0)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
                return true;
            }

            return false;
        }

        #endregion

        protected void InteractWithNearInteractables()
        {
            Debug.Log("Switch button");
            Collider2D[] colls = new Collider2D[20];
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = true;
            GetComponent<Collider2D>().OverlapCollider(contactFilter, colls);

            foreach (Collider2D coll in colls)
            {
                coll?.GetComponent<Interactable>()?.SwitchOnOff();
            }
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
