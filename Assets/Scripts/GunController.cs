namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GunController : MonoBehaviour
    {

        [SerializeField]
        [Range(0.1f, 1.5f)]
        private float gunCooldown = 0.3f;

        [SerializeField]
        private float flashShotDuration = 0.075f;

        [SerializeField]
        private MeshRenderer flashShot;

        [SerializeField]
        private GameObject bulletPrefab;

        private bool shotInCooldown = false;

        private void Start()
        {
            flashShot.enabled = false;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!shotInCooldown && Input.GetMouseButtonDown(0))
            {
                Fire();
            }

            LookTowardMouse();
        }

        private void LookTowardMouse()
        {
            //away?

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 perpendicular = mousePos - transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
        }

        private void Fire()
        {
            SpawnBullet();
            StartCoroutine(_FlashShot());
            StartCoroutine(_ShotCooldown());
        }

        private void SpawnBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, LevelManager.Instance.BulletsParent);
            bullet.transform.position = transform.position; //nuzzle
            bullet.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);

            //direction is mouse
            bullet.GetComponent<Bullet>().Direction = GetMouseDirection();

            bullet.SetActive(true);

        }

        private Vector2 GetMouseDirection()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return mousePos - transform.position;
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
