﻿using Light2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FlickerLight : MonoBehaviour
{
    [SerializeField]
    [Range(0, 0.3f)]
    private float lightOnDuration;
    [SerializeField]
    [Range(0, 4)]
    private float lightOffDuration;

    [SerializeField]
    private bool randomFlicker = false;

    [SerializeField]
    private Vector2 randomFlickerRange;

    private float timer = 0f;
    private float duration = 0f;
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        mr.enabled = false;
        duration = lightOffDuration;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > duration)
        {
            SwitchLight();
        }
    }

    public void SwitchLight()
    {
        if (mr.enabled)
        {
            if (randomFlicker)
            {
                duration = Random.Range(randomFlickerRange.x, randomFlickerRange.y);
            }
            else
            {
                duration = lightOffDuration;
            }
        }
        else
        {
            duration = lightOnDuration;
        }

        mr.enabled = !mr.enabled;
        timer = 0f;
    }
}