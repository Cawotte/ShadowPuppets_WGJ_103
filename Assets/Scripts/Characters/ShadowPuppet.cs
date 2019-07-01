namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ShadowPuppet : Character
    {

        PuppetCollider puppetColl;

        protected override void Awake()
        {
            base.Awake();
            puppetColl = GetComponentInChildren<PuppetCollider>();
            puppetColl.OnPlayerContact += (charac) => charac.DealDamage(ContactDamage);
        }
        
    }

}
