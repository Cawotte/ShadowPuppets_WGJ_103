using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerGameObject : MonoBehaviour
{
    [SerializeField]
    Transform target;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
            UnityEditor.Handles.Label((transform.position + target.position) / 2,
                Vector3.Distance(transform.position, target.position).ToString()
                );
        }
    }
#endif
}
