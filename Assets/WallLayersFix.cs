using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallLayersFix : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x,
            other.gameObject.transform.position.y, 1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x,
            other.gameObject.transform.position.y, 0);
    }
}
