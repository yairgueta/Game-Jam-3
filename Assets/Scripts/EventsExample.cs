using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum jcds{miri, yair, elran}


public class EventsExample : MonoBehaviour
{
    public UnityEvent<jcds> onZibiZibi;
    
    void Start()
    {
        onZibiZibi.AddListener((arg0 => Debug.Log(arg0)));
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     onZibiZibi?.Invoke(true);
        // }
        // if (Input.GetKey(KeyCode.A))
        // {
        //     onZibiZibi?.Invoke(false);
        // }
        // if (Input.GetKey(KeyCode.S))
        // {
        //     onZibiZibi?.Invoke(false);
        // }
        // if (Input.GetKey(KeyCode.D))
        // {
        //     onZibiZibi?.Invoke(true);
        // }
        // if (Input.GetKey(KeyCode.F))
        // {
        //     onZibiZibi?.Invoke(false);
        // }
        
    }

    
}
