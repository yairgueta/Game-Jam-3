using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirisCollectible : MonoBehaviour
{

    private bool isNextToPlayer = false;

    private void OnMouseOver()
    {
        if (isNextToPlayer)
        {
            Debug.Log("MIRI");
        }
    }

    public void GetNearPlayer()
    {
        isNextToPlayer = true;
    }
    
    
}
