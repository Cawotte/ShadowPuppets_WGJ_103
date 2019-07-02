namespace WGJ.PuppetShadow
{
    using Light2D;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShadowPuppet : Puppet
    {
        [Header("Shadow Puppet")]

        [SerializeField]
        private int contactDamage = 1;
        [SerializeField]
        GameObject lightPuppetPrefab;
        
        PuppetCollider puppetColl;
        

        protected override void Awake()
        {
            base.Awake();
            puppetColl = GetComponentInChildren<PuppetCollider>();
            puppetColl.OnPlayerContact += (charac) => charac.DealDamage(contactDamage);

            OnDeath += SpawnLightPuppet;
        }

        private void Start()
        {
            stateMachine = new StateMachine(new StatePursuit(), this);
        }

        public void SpawnLightPuppet()
        {
            //Spawn at same place
            LightPuppet lightPuppet = Instantiate(lightPuppetPrefab, transform.parent).GetComponent<LightPuppet>();
            lightPuppet.transform.position = transform.position;
            lightPuppet.transform.rotation = transform.rotation;
            lightPuppet.transform.localScale = transform.localScale;

            lightPuppet.gameObject.SetActive(true);
        }
    }
}
