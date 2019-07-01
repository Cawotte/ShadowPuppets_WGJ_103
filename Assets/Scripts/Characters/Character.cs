namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class Character : MonoBehaviour
    {

        private Rigidbody2D rb;
        [SerializeField]
        private int totalLife;
        [SerializeField]
        private int currentLife;

        [SerializeField]
        protected float speed = 2.5f;
        [SerializeField]
        protected float jumpStrenght = 6f;

        [SerializeField]
        private int contactDamage = 0;


        [Range(0, .3f)]
        [SerializeField]
        private float movementSmoothing = .13f; // How much to smooth out the movement

        protected bool isInvincible = false;
        //Events
        public Action<int> OnDamageTaken = null;
        public Action<int> OnHeal = null;
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
        
        public int TotalLife { get => totalLife;  }
        public int ContactDamage { get => contactDamage; }

        #endregion
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            currentLife = totalLife;

            OnDamageTaken += (num) => StartCoroutine(_RedBlink());
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
            OnDamageTaken?.Invoke(damage);

            if (CurrentLife <= 0)
            {
                OnDeath?.Invoke();
                Debug.Log(gameObject.name + "dies.");
                Destroy(gameObject);
                Debug.Log(gameObject.name + "dead.");
                return true;
            }

            return false;
        }

        public void Heal(int heal)
        {
            CurrentLife += heal;
            OnHeal?.Invoke(heal);
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

        protected IEnumerator _RedBlink()
        {
            float timer = 0f;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color originalColor = sr.color;
            sr.color = Color.red;

            while (timer < 0.1f)
            {
                yield return null;
                timer += Time.fixedDeltaTime;
            }

            sr.color = originalColor;

        }
    }

}
