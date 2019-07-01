namespace WGJ.PuppetShadow {

    
    using UnityEngine;

    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private float distanceMinSpeed = 0.5f;
        

        private Vector3 TargetPosition {
            get => new Vector3(
                target.position.x, 
                target.position.y, 
                transform.position.z);
        }

        // Start is called before the first frame update
        void Start()
        {
            if (target == null)
                target = LevelManager.Instance.Player.transform;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (!CameraIsOnTarget())
            {
                MoveCameraTowards(target);
            }
        }

        private void MoveCameraTowards(Transform target)
        {
            //Exponential falloff

            float distance = Vector3.Distance(transform.position, TargetPosition);
            Vector3 direction = (TargetPosition - transform.position);

            if (direction.sqrMagnitude < distanceMinSpeed)
            {
                direction = direction.normalized * distanceMinSpeed;
            }

            Vector3 movement = direction * (1 - Mathf.Exp(-Time.deltaTime * speed));
            

            transform.position += movement;
        }

        private bool CameraIsOnTarget()
        {
            return Vector3.Distance(transform.position, TargetPosition) < 0.1f;
        }
    }
}
