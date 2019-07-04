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
        protected AudioSourcePlayer soundPlayer;

        [SerializeField]
        private int totalLife;
        [SerializeField]
        [ReadOnly]
        private int currentLife;

        [SerializeField]
        protected float speed = 12.5f;

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

        //asset flip
        protected bool isFacingRight;

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
        public AudioSourcePlayer SoundPlayer { get => soundPlayer; }
        
        #endregion

        #region MonoBehaviour Loop
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            currentLife = totalLife;

            OnDamageTaken += (num) => RedBlink();
            OnHeal += (num) => GreenBlink();

            if (soundPlayer == null)
            {
                soundPlayer = gameObject.AddComponent<AudioSourcePlayer>();
            }


        }

        private void Start()
        {


            UnityEditor.Selection.activeGameObject = gameObject;
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
                if (this is PlayerCharacter)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                }
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

        protected void FacePos(Vector3 PosToFace)
        {
            Vector3 direction = PosToFace - transform.position;

            if (direction.x < 0 && isFacingRight)
            {
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                //GetComponent<SpriteRenderer>().flipX = true;
                isFacingRight = false;
            }
            else if (direction.x > 0 && !isFacingRight)
            {
                transform.rotation *= Quaternion.Euler(0, 180, 0);
                //GetComponent<SpriteRenderer>().flipX = false;
                isFacingRight = true;
            }
        }

        protected void MoveToward(Vector2 direction)
        {
            Vector3 movement = direction * Time.fixedDeltaTime * speed;
            rb.MovePosition(transform.position + movement);
        }

        private void RedBlink() {
            StartCoroutine(_ColorBlink(Color.red));
        }

        private void GreenBlink()
        {
            StartCoroutine(_ColorBlink(Color.green));
        }
        
        
        protected IEnumerator _ColorBlink(Color color)
        {
            float timer = 0f;

            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = color;

            while (timer < 0.1f)
            {
                yield return null;
                timer += Time.fixedDeltaTime;
            }

            sr.color = Color.white;

            

        }



    }

}
