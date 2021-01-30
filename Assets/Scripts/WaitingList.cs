using System;
using System.Collections.Generic;
using UnityEngine;

public class WaitingList
{
    private List<bool> waitingList;
    private bool canRegisterToWaitingList = true;
    private Action onFinishLoad;

    public WaitingList(Action onFinishLoad)
    {
        waitingList = new List<bool>();
        this.onFinishLoad = onFinishLoad;
    }

    public void Clear()
    {
        waitingList.Clear();
        canRegisterToWaitingList = true;
    }

    public Action Register()
    {
        if (!canRegisterToWaitingList)
        {
            Debug.LogError("Cannot register to waiting list after game startup!");
            return null;
        }
        var i = waitingList.Count;
        waitingList.Add(false);
        return () => OnFinishedTask(i);
    }
    
    public bool AreAllDone => !waitingList.Contains(false);
    
    private void OnFinishedTask(int i)
    {
        waitingList[i] = true;
        if (AreAllDone) onFinishLoad?.Invoke();
    }

}