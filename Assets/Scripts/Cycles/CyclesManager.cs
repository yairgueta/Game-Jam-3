using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// represents the type of cycles in the game.
public enum Cycles
{
    Day,
    Magic,
    Night
}

// updates the current game cycle.
public class CyclesManager : Singleton<CyclesManager>
{
    [SerializeField] public UnityEvent<Cycles> onCycleChange;
    private Dictionary<Cycles, float> cyclesDurations = new Dictionary<Cycles, float>()
    {
        {Cycles.Day, 10f},
        {Cycles.Night, 5f},
        {Cycles.Magic, 2f},
    };

    [Tooltip("Initial game mode")] [SerializeField]
    private Cycles currentTimeMode; // Serialized to be able to determine the initial mode.

    private float currentTimeCount;
    private float initialTime;


    // Start is called before the first frame update
    void Start()
    {
        initialTime = Time.time;
        onCycleChange?.Invoke(currentTimeMode);
    }

    // Update is called once per frame
    void Update()
    {
        IncrementTime();
        TrackMode();
    }

    // increments the time counter of the current game mode, by the time elapsed from the last incrementation.
    private void IncrementTime()
    {
        currentTimeCount += Time.time - (currentTimeCount + initialTime);
    }

    // tracks the mode of the game, and changes it if the time has come.
    private void TrackMode()
    {
        if (!(currentTimeCount >= cyclesDurations[currentTimeMode])) return;
        initialTime += cyclesDurations[currentTimeMode];
        currentTimeCount = 0;
        switch (currentTimeMode)
        {
            case Cycles.Day:
                currentTimeMode = Cycles.Night;
                onCycleChange?.Invoke(Cycles.Night);
                break;
            case Cycles.Night:
                currentTimeMode = Cycles.Magic;
                onCycleChange?.Invoke(Cycles.Magic);
                break;
            case Cycles.Magic:
                currentTimeMode = Cycles.Day;
                onCycleChange?.Invoke(Cycles.Day);
                break;
        }
    }

    // returns the time count of the current game cycle.
    public float GetTimeInCurrentCycle()
    {
        return (currentTimeCount) / cyclesDurations[currentTimeMode];
        //TODO: Improve performance here!
    }
}