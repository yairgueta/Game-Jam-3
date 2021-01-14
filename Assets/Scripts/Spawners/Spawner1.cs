using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner1 : MonoBehaviour
{
    private GridGraph graphs;
    private AstarData astarData;

    private void Start()
    {
        astarData = AstarPath.active.data;
        // graphs = astarData.graphTypes.
    }
}
