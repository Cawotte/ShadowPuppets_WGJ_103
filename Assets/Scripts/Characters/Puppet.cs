namespace WGJ.PuppetShadow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Puppet : Character
    {

        [SerializeField]
        protected bool isPathing = false;
        protected Vector3 targetPos;

        [SerializeField]
        protected float deviationStrenght = 1.5f;
        [SerializeField]
        protected float closeRangeValue = 0.3f;
        [SerializeField]
        protected float cornerBrake = 0.3f;

        [SerializeField]
        [ReadOnly]
        private Vector2 velocity;

        [SerializeField]
        [Range(0, 0.5f)]
        private float coefVelocity;

        [SerializeField]
        [ReadOnly]
        private float observedSpeed;

        protected void MoveTo(Vector3 targetWorldPos)
        {
            TilePath path = LevelManager.Instance.Pathfinder.GetPath(transform.position, targetWorldPos);

            if (path.IsEmpty)
                return;

            StartCoroutine(_FollowPath(path.SimplifiedPath));
        }
        protected IEnumerator _FollowPath(TilePath path)
        {
            isPathing = true;
            Vector3 previousPos = transform.position;

            for (int i = 0; i < path.Size; i++)
            {
                targetPos = path[i].CenterWorld;
                rb.velocity = GetGhostVelocityToward(previousPos, targetPos);
                while (!IsVeryCloseTo(path[i].CenterWorld))
                {
                    yield return null;
                    rb.velocity = GetGhostVelocityToward(previousPos, targetPos);
                    velocity = rb.velocity;
                    observedSpeed = velocity.magnitude;
                }
                previousPos = transform.position;
            }
            rb.velocity = Vector2.zero;
            /*
            for (int i = 0; i < path.Size; i++)
            {
                targetPos = path[i].CenterWorld;
                rb.velocity = GetGhostVelocityToward(previousPos, targetPos);
                while (!IsVeryCloseTo(path[i].CenterWorld))
                {
                    yield return null;
                    rb.velocity = GetGhostVelocityToward(previousPos, targetPos);
                    velocity = rb.velocity;
                    observedSpeed = velocity.magnitude;
                }
            }
            rb.velocity = Vector2.zero;
            */
            isPathing = false;
        }

        protected bool IsVeryCloseTo(Vector3 worldPos)
        {
            return Vector3.Distance(transform.position, worldPos) < closeRangeValue;
        }

        private Vector2 GetGhostVelocityToward(Vector3 previousPos, Vector3 targetPos)
        {
            float adjustedSpeed = speed;
            float perpendicularCoeff = 1f;

            float distanceFromNext = Vector3.Distance(transform.position, targetPos);
            float distanceFromPrevious = Vector3.Distance(transform.position, previousPos);
            float distanceTotal = Vector3.Distance(targetPos, previousPos);


            float t = distanceFromNext / distanceTotal;

            /*
            if (distanceFromPrevious < 1 + closeRangeValue)
            {
                adjustedSpeed = Mathf.Lerp(speed, speed * cornerBrake, distanceFromPrevious - closeRangeValue);
                perpendicularCoeff = Mathf.Lerp(1f, 0f, distanceFromPrevious - closeRangeValue);
            }
            else if (distanceFromNext < 1 + closeRangeValue)
            {
                adjustedSpeed = Mathf.Lerp(speed * cornerBrake, speed, distanceFromNext - closeRangeValue);
                perpendicularCoeff = Mathf.Lerp(0f, 1f, distanceFromNext - closeRangeValue);
            }*/

            Vector3 remnantVelocity = rb.velocity * coefVelocity;
            Vector3 direction = (remnantVelocity + (targetPos - transform.position).normalized).normalized * adjustedSpeed;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;


            return remnantVelocity + (direction + (perpendicular * (Mathf.Pow(Mathf.Cos(t),2f)-0.5f) * deviationStrenght * perpendicularCoeff));


        }
        /*
        private Vector2 GetGhostVelocityToward(int index, TilePath path)
        {
            float adjustedSpeed = speed;
            float perpendicularCoeff = 1f;

            float distanceFromNext = Vector3.Distance(transform.position, targetPos);
            float distanceFromPrevious = Vector3.Distance(transform.position, previousPos);
            float distanceTotal = Vector3.Distance(targetPos, previousPos);


            float t = distanceFromNext / distanceTotal;

            /*
            if (distanceFromPrevious < 1 + closeRangeValue)
            {
                adjustedSpeed = Mathf.Lerp(speed, speed * cornerBrake, distanceFromPrevious - closeRangeValue);
                perpendicularCoeff = Mathf.Lerp(1f, 0f, distanceFromPrevious - closeRangeValue);
            }
            else if (distanceFromNext < 1 + closeRangeValue)
            {
                adjustedSpeed = Mathf.Lerp(speed * cornerBrake, speed, distanceFromNext - closeRangeValue);
                perpendicularCoeff = Mathf.Lerp(0f, 1f, distanceFromNext - closeRangeValue);
            }

            Vector3 remnantVelocity = rb.velocity * coefVelocity;
            Vector3 direction = (remnantVelocity + (targetPos - transform.position).normalized).normalized * adjustedSpeed;
            Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;


            return remnantVelocity + (direction + (perpendicular * (Mathf.Pow(Mathf.Cos(t), 2f) - 0.5f) * deviationStrenght * perpendicularCoeff));


        }*/

        private float GetVelocity(int index, TilePath path)
        {
            Func<float, Vector3> fun = GetDestinationFunction(index, path);

            return 0f;

        }

        private Func<float, Vector3> GetDestinationFunction(int index, TilePath path)
        {
            Vector3 start = path[index].CenterWorld;
            if (index - path.Size < 3)
            {
                return (t) => Vector3.Lerp(start, path.Goal.CenterWorld, t);
            }
            else
            {
                return (t) => InterpolateQuadraticBezierCurve(
                    path[index].CenterWorld, 
                    path[index + 1].CenterWorld, 
                    path[index + 2].CenterWorld, 
                    t);
            }
        }

        private Func<float, Vector3> GetBezierTurn(Vector3 turnCenter, Vector3 nextPoint)
        {
            Vector3 A = turnCenter - transform.position;
            Vector3 C = (nextPoint - turnCenter).normalized * A.magnitude;

            return (t) => InterpolateQuadraticBezierCurve(A, turnCenter, C, t);
        }
        private Vector3 InterpolateQuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            return p0 * Mathf.Pow(1f - t, 2) + 
                p1 * 2 * (1 - t) * t + 
                p2 * Mathf.Pow(t, 2);
        }

        private void OnDrawGizmos()
        {
            if (targetPos != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, targetPos);
            }
        }
    }
}
