namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class OriginalPuppet : Character
    {
        [SerializeField]
        private GameObject shadowPuppetPrefab;

        private ShadowPuppet shadowPuppet;

        protected override void Awake()
        {
            base.Awake();
            //OnDeath += DestroyShadowPuppet;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void DestroyShadowPuppet()
        {
            shadowPuppet.DealDamage(shadowPuppet.TotalLife);
        }

        private void CreateShadowPuppet()
        {
            shadowPuppet = Instantiate(shadowPuppetPrefab, transform).GetComponent< ShadowPuppet>();
            shadowPuppet.transform.position += Vector3.left * 3f;


        }
    }

}
