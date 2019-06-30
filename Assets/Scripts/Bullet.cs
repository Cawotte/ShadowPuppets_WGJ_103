namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Bullet : MonoBehaviour
    {

        [SerializeField] private float velocity = 1f;
        [SerializeField] private float maxLifetime = 3f;

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
            if (!collision.tag.Equals("Player"))
            {
                Destroy(gameObject);
            }
        }
    }

}
