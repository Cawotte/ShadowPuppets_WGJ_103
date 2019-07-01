namespace WGJ.PuppetShadow
{
    using Light2D;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShadowPuppet : Character
    {
        [SerializeField]
        private int healAmount = 2;

        [SerializeField]
        GameObject lightPuppetPrefab;


        PuppetCollider puppetColl;
        

        protected override void Awake()
        {
            base.Awake();
            puppetColl = GetComponentInChildren<PuppetCollider>();
            puppetColl.OnPlayerContact += (charac) => charac.DealDamage(ContactDamage);
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
