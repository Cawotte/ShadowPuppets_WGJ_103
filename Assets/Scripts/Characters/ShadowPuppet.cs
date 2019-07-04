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
        [SerializeField]
        GameObject lifeforcePrefab;

        
        PuppetCollider puppetColl;

        private OriginalPuppet originalPuppet;

        public OriginalPuppet OriginalPuppet { get => originalPuppet; set => originalPuppet = value; }

        protected override void Awake()
        {
            base.Awake();
            puppetColl = GetComponentInChildren<PuppetCollider>();
            puppetColl.OnPlayerContact += (charac) => charac.DealDamage(contactDamage);

            OnDeath += SpawnLightPuppet;
        }

        private void Start()
        {
            if (LevelManager.Instance.Player != null)
                OnDeath += LevelManager.Instance.Player.PlayGhostDeathSound;
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


        public void OnShadowContact()
        {
            soundPlayer.PlayRandomFromList("puppetWhiff");
            InstantiateLifeforce();

        }

        private void InstantiateLifeforce()
        {
            LifeForce lifeforce = Instantiate(lifeforcePrefab, transform.position, transform.rotation).GetComponent < LifeForce>(); ;

            lifeforce.Origin = transform;
            lifeforce.Target = originalPuppet.transform;

            lifeforce.gameObject.SetActive(true);
        }
    }
}
