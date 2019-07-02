namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class Character : MonoBehaviour
    {
        [Header("General Character")]
        protected Rigidbody2D rb;

        [SerializeField]
        private int totalLife;
        [SerializeField]
        [ReadOnly]
        private int currentLife;

        [SerializeField]
        protected float speed = 2.5f;

        [Header("Status")]

        [SerializeField]
        protected bool isInvincible = false;
        [SerializeField]
        [ReadOnly]
        protected bool isMoving = false;

        //Events
        public Action<int> OnDamageTaken = null;
        public Action<int> OnHeal = null;
        public Action<int> OnLifeChange = null;
        public Action OnDeath = null;


        [Header("Debug Infos")]
        //internals var
        [SerializeField]
        [ReadOnly]
        protected Vector2 velocity = Vector2.zero;
        [SerializeField]
        [ReadOnly]
        protected float observedSpeed;


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
        public bool IsMoving { get => isMoving; }

        #endregion

        #region MonoBehaviour Loop
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            currentLife = totalLife;

            OnDamageTaken += (num) => StartCoroutine(_RedBlink());
        }

        protected virtual void LateUpdate()
        {
            //observe if moving.
            isMoving = (rb.velocity != Vector2.zero);

            velocity = rb.velocity;
            observedSpeed = rb.velocity.magnitude;
        }

        #endregion

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
                Destroy(gameObject);
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

        protected void MoveToward(Vector2 direction)
        {
            Vector3 movement = direction * Time.fixedDeltaTime * speed;
            rb.MovePosition(transform.position + movement);
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
