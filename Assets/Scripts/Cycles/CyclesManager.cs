using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public enum Cycles {Day, Twilight, Night}

public class CyclesManager : MonoBehaviour
{

    [SerializeField] private SerializedDictionary<Cycles, float> cyclesDurations =
        new SerializedDictionary<Cycles, float>
        {
            {Cycles.Day, 10f}, {Cycles.Twilight, 5f}, {Cycles.Night, 10f}
        };

    private float currentTime;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
