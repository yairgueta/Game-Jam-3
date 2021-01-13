using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using Events;
using UnityEngine;

[CreateAssetMenu(menuName = "Collectables/Mushroom_@@")]

public class MushroomCollectable : CollectableObject
{
    public GameEvent onMushroomCollect;


    public override void OnCollected()
    {
        onMushroomCollect.Raise();
    }
}
