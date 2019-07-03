namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class OriginalPuppet : Puppet
    {
        [Header("Original Puppet")]
        [SerializeField]
        private GameObject shadowPuppetPrefab;

        private ShadowPuppet shadowPuppet;

        protected override void Awake()
        {
            base.Awake();
            OnDamageTaken += (num) => soundPlayer.PlayRandomFromList("puppetDamaged");
            OnDeath += DestroyShadowPuppet;
            OnDeath += () => LevelManager.Instance.CurrentPuppets--;
            OnDeath += () => LevelManager.Instance.Score++;
        }
        private void Start()
        {
            LevelManager.Instance.CurrentPuppets++;
            CreateShadowPuppet();
            stateMachine = new StateMachine(new StateHide(), this);
        }
        
        private void DestroyShadowPuppet()
        {
            shadowPuppet.DealDamage(shadowPuppet.TotalLife);
            //Destroy(shadowPuppet.gameObject);
        }

        private void CreateShadowPuppet()
        {
            shadowPuppet = Instantiate(shadowPuppetPrefab, transform.parent).GetComponent<ShadowPuppet>();
            shadowPuppet.transform.position = transform.position;
            shadowPuppet.transform.rotation = transform.rotation;
            shadowPuppet.transform.localScale = transform.localScale;
            //shadowPuppet.transform.position += Vector3.right * 3f;

            shadowPuppet.gameObject.SetActive(true);


        }
    }

}
