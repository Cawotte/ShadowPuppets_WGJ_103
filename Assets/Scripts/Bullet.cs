namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {

        [SerializeField] private float velocity = 1f;
        [SerializeField] private float maxLifetime = 3f;
        [SerializeField] private int damage = 1;

        private Rigidbody2D rb;
        private Vector2 direction = Vector2.zero;
        private float timerLifetime = 0f;

        public Vector2 Direction { get => direction; set => direction = value; }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveIn(direction);

            if (timerLifetime >= maxLifetime)
            {
                Destroy(gameObject);
                return;
            }

            timerLifetime += Time.fixedDeltaTime;
        }

        private void MoveIn(Vector2 direction)
        {
            Vector3 movement = direction * Time.fixedDeltaTime * velocity;
            rb.MovePosition(transform.position + movement);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character character = collision.GetComponent<Character>();

            //If it's a trigger or the player, ignore it.
            if (collision.isTrigger && character == null)
            {
                return;
            }
            
            if (character != null)
            {
                /*
                if (!character.IsTangible)
                {
                    return;
                }*/
                character.DealDamage(damage);
            }

            //Destroy on touch
            Destroy(gameObject);
        }
    }

}
