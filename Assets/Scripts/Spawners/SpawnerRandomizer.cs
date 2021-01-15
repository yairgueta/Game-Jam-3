using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spawners
{
    public class SpawnerRandomizer : MonoBehaviour
    {
        private static readonly int MAX_ITERATIONS = 50;

        private GridGraph[] graphs;
        private AstarData astarData;
        private Collider2D boundaries;

        private void Awake()
        {
            boundaries = GetComponent<Collider2D>();
        }

        private void Start()
        {
            astarData = AstarPath.active.data;
            graphs = astarData.FindGraphsOfType(typeof(GridGraph)).OfType<GridGraph>().ToArray();
        }


        private bool IsFree(Bounds bounds)
        {
            var ls = new List<GraphNode>[graphs.Length];
            var walkable = true;
            for (var i = 0; i < graphs.Length; i++)
            {
                var graph = graphs[i];
                var nodes = graph.GetNodesInRegion(bounds);
                walkable = nodes.Aggregate(walkable, (current, node) => current && node.Walkable);
                ls[i] = nodes;
            }

            Debug.Log(bounds);

            if (walkable) ls[0].ForEach(g => g.Walkable = false);

            foreach (var graphNodes in ls)
            {
                ListPool<GraphNode>.Release(graphNodes);
            }

            return walkable;
        }

        public void RandomizeObjectPosition(GameObject go)
        {
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                var newPos = GetRandomPointWithinSpawner();
                go.transform.position = newPos;
                var collider = go.GetComponent<Collider2D>();
                if (IsFree(new Bounds(newPos, collider.bounds.size)))
                    return;
            }

            Debug.LogError(gameObject.name + " Couldn't find free space for " + go.name);
        }


        private Vector2 GetRandomPointWithinSpawner()
        {
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                var point = new Vector2(
                    Random.Range(boundaries.bounds.min.x, boundaries.bounds.max.x),
                    Random.Range(boundaries.bounds.min.y, boundaries.bounds.max.y)
                );

                if (point == boundaries.ClosestPoint(point)) return point;
                Debug.Log(i);
            }

            Debug.LogError("Bad Luck at finding point inside a collider!");
            return new Vector2();
        }

    }
}