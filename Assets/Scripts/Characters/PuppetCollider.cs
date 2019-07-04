namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PuppetCollider : MonoBehaviour
    {
        [SerializeField]
        private float contactRefresh = 1f;

        public Action<Character> OnPlayerContact = null;

        private float cooldown = 0f;

        private Puppet parentPuppet;

        private void Start()
        {
            parentPuppet = GetComponentInParent<Puppet>();
        }
        private void FixedUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.fixedDeltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            OnShadowPuppetContact(collision);
        }
        private void OnTriggerStay2D(Collider2D collision)
        {


            PlayerCharacter player = collision.GetComponent<PlayerCharacter>();

            if (player == null)
            {
                return;
            }

            if (cooldown <= 0f)
            {
                OnPlayerContact?.Invoke(player);
                cooldown = contactRefresh;
            }
        }

        private void OnShadowPuppetContact(Collider2D collision)
        {
            if (!(parentPuppet is ShadowPuppet) || collision.gameObject.layer != LayerMask.NameToLayer("Bullet"))
                return;

            ShadowPuppet shadow = (ShadowPuppet)parentPuppet;

            shadow.OnShadowContact();


        }
        
    }

}
