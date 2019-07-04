namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GunController : MonoBehaviour
    {

        [HideInInspector] public PlayerCharacter Player; //ugly

        [SerializeField]
        [Range(0.1f, 1.5f)]
        private float gunCooldown = 1f;

        [SerializeField]
        private float flashShotDuration = 0.075f;

        [SerializeField]
        private MeshRenderer flashShot;

        [SerializeField]
        private GameObject bulletPrefab;
        [SerializeField]
        private Transform nuzzle;

        private bool isFacingRight = true;
        private bool shotInCooldown = false;
        private Vector2 mouseDirection = Vector2.zero;

        private void Start()
        {
            flashShot.enabled = false;
            Player = GetComponentInParent<PlayerCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (UIManager.Instance.PauseIsEnabled) return;

            if (!shotInCooldown && Input.GetMouseButtonDown(0))
            {
                Fire();
            }

            AimTowardMouse();
        }

        private void AimTowardMouse()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, GetMouseDirection()) * Quaternion.Euler(0, 0, -90);
            AdjustRotationGun();
        }

        private void AdjustRotationGun()
        {
            float x = GetMouseDirection().x;

            isFacingRight = x > 0f;
            if (isFacingRight)
            {
                transform.rotation *= Quaternion.Euler(180, 0, 0);
            }
        }
        private void Fire()
        {
            Player.SoundPlayer.PlayRandomFromList("pistolShot");
            SpawnBullet();
            StartCoroutine(_FlashShot());
            StartCoroutine(_ShotCooldown());
        }

        private void SpawnBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, LevelManager.Instance.BulletsParent);
            bullet.transform.position = nuzzle.position; //nuzzle
            bullet.transform.rotation = transform.rotation;

            //direction is mouse
            bullet.GetComponent<Bullet>().Direction = GetMouseDirection().normalized * -1f;

            bullet.SetActive(true);

        }

        private Vector2 GetMouseDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return transform.position - mousePos;
        }

        private IEnumerator _FlashShot()
        {
            float timer = 0f;
            flashShot.enabled = true;

            while (timer < flashShotDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            flashShot.enabled = false;
        }

        private IEnumerator _ShotCooldown()
        {
            float timer = 0f;
            shotInCooldown = true;

            while (timer < gunCooldown)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            shotInCooldown = false;
        }

    }
}
