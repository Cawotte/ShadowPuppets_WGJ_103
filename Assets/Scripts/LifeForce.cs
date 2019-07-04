namespace WGJ.PuppetShadow
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LifeForce : MonoBehaviour
    {

        public Transform Origin;
        public Transform Target;

        public float speed = 3f;
        
        private void Start()
        {
            if (Origin == null || Target == null)
            {
                Debug.Log("Lifeforce killed at birth!");
                Destroy(gameObject);
            }
            

            StartCoroutine(_FlyFromTo(Origin.position + Vector3.up, Target.position + Vector3.up));
        }

        public IEnumerator _FlyFromTo(Vector2 a, Vector2 b)
        {
            float speedMultiplier;
            float step = (speed / (a - b).magnitude) * Time.deltaTime;
            float t = 0;
            while (t <= 1.0f)
            {

                speedMultiplier = Mathf.Lerp(0, Mathf.PI / 2, t);
                speedMultiplier = Mathf.Sin(speedMultiplier) + 1f;

                step = ( (speed * speedMultiplier) / (a - b).magnitude) * Time.deltaTime;

                t += step; // Goes from 0 to 1, incrementing by step each time


                transform.position = Vector3.Lerp(a, b, t); // Move objectToMove closer to b
                yield return null;         // Leave the routine and return here in the next frame

                if (Origin == null || Target == null)
                {
                    Destroy(gameObject);
                }
            }
            Destroy(gameObject);
        }

        public float speedFunction(float t)
        {

            //sin(O) to (sin(pi))
            float lerp = Mathf.Lerp(0, Mathf.PI, t * Time.deltaTime);

            return Mathf.Sin(lerp);
        }
    }
}
