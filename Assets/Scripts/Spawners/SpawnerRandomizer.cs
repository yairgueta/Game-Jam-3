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

        [SerializeField] private Vector2 perlinVar;
        [SerializeField] private float scale = .1f;
        
        private GridGraph[] graphs;
        private AstarData astarData;
        private Collider2D boundaries;
        private Vector2 worldSpaceOffset;
        
        private void Awake()
        {
            boundaries = GetComponent<Collider2D>();
        }

        private void Start()
        {
            astarData = AstarPath.active.data;
            graphs = astarData.FindGraphsOfType(typeof(GridGraph)).OfType<GridGraph>().ToArray();
            worldSpaceOffset = boundaries.offset + (Vector2)transform.position;
            perlinVar = new Vector2(Random.value, Random.value) * 100f;
        }


        private List<GraphNode> IsFree(Bounds bounds)
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
            
            foreach (var graphNodes in ls.Skip(1))
            {
                ListPool<GraphNode>.Release(graphNodes);
            }

            return walkable ? ls[0] : null;
        }

        private void AnyRandomizeObjectPosition(Spawnable spnble, Func<Vector2> randomMethod)
        {
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                var newPos = randomMethod();
                spnble.transform.position = newPos;
                var spnbleCollidr = spnble.physicsCollider;
                var takenNodes = IsFree(new Bounds(newPos + spnbleCollidr.offset, spnbleCollidr.bounds.size));
                if (takenNodes != null)
                {
                    spnble.takenNodes = takenNodes;
                    return;
                }
            }

            Debug.LogError(gameObject.name + " Couldn't find free space for " + spnble.name);
        }

        public void RandomizeObjectPosition(Spawnable spnble) =>
            AnyRandomizeObjectPosition(spnble, GetRandomPointWithinSpawner);

        public void PerlinRandomizeObjectPosition(Spawnable spnble) =>
            AnyRandomizeObjectPosition(spnble, GetRandomPointWithinSpawnerPerlinNoise);


        private Vector2 GetRandomPointWithinSpawnerPerlinNoise()
        {
            var threshold = .5f;
            for (int i = 0; i < MAX_ITERATIONS * 2; i++)
            {
                var point = new Vector2(
                    Random.Range(boundaries.bounds.min.x, boundaries.bounds.max.x),
                    Random.Range(boundaries.bounds.min.y, boundaries.bounds.max.y)
                );
                var perlinNoise = Mathf.PerlinNoise(
                    perlinVar.x + (point.x - worldSpaceOffset.x) / (scale * boundaries.bounds.size.x),
                    perlinVar.y + (point.y - worldSpaceOffset.y) / (scale * boundaries.bounds.size.y));
                if (perlinNoise < threshold) continue;
                if (point == boundaries.ClosestPoint(point)) return point;
            }

            Debug.LogError("Bad Luck at finding point inside a collider and perlin noise!");
            return new Vector2();
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private Vector2 GetRandomPointWithinSpawner()
        {
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                var point = new Vector2(
                    Random.Range(boundaries.bounds.min.x, boundaries.bounds.max.x),
                    Random.Range(boundaries.bounds.min.y, boundaries.bounds.max.y)
                );

                if (point == boundaries.ClosestPoint(point)) return point;
            }

            Debug.LogError("Bad Luck at finding point inside a collider!");
            return new Vector2();
        }

    }
}