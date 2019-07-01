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

        private void FixedUpdate()
        {
            if (cooldown > 0)
            {
                cooldown -= Time.fixedDeltaTime;
            }
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
        
    }

}
