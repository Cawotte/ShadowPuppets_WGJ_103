namespace WGJ.PuppetShadow
{
    using Light2D;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LightPuppet : Character
    {
        [Header("Light Puppet")]
        [SerializeField]
        private int healAmount = 2;

        PuppetCollider puppetColl;

        protected override void Awake()
        {
            base.Awake();
            puppetColl = GetComponentInChildren<PuppetCollider>();
            puppetColl.OnPlayerContact += (player) => player.Heal(healAmount);
            puppetColl.OnPlayerContact += (player) => Destroy(gameObject); //self destruct on heal
        }

        private void Start()
        {
            StartCoroutine(_LateLight());
        }

        private IEnumerator _LateLight()
        {

            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;

            GetComponentInChildren<LightObstacleSprite>().gameObject.layer = LightingSystem.Instance.LightSourcesLayer;

        }
    }
}
