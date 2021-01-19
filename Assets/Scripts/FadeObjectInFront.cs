using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectInFront : MonoBehaviour
{
    [SerializeField] private LayerMask layersToFade;

    private void FixedUpdate()
    {
        var hit = Physics2D.Raycast(transform.position, Vector3.forward, 10f, layersToFade);
        
        
    }
}
