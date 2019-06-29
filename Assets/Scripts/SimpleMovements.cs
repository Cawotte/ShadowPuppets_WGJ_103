using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovements : MonoBehaviour
{

    [SerializeField]
    private float speed = 1f;

    private Vector2 direction;

    // Update is called once per frame
    void Update()
    {
        direction = GetDirectionFromAxis();
        if (direction != Vector2.zero)
        {
            Vector3 movement = direction * Time.deltaTime * speed;
            transform.position += movement;

        }

        LookTowardMouse();

    }

    private Vector2 GetDirectionFromAxis()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
    }

    private void LookTowardMouse()
    {
        //away?

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perpendicular = mousePos - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
    }
}
