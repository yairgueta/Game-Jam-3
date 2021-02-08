using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LoadNavMesh : MonoBehaviour
{
    private NavMeshSurface2d navMeshSurface;

    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface2d>();
        StartCoroutine(BakeNavMesh());
    }

    IEnumerator BakeNavMesh()
    {
        var data = InitializeBakeData();
        var async = navMeshSurface.UpdateNavMesh(data);

        while (!async.isDone)
        {
            print("loading");
            yield return null;
        }
        print("finished");
        navMeshSurface.navMeshData = data;
        navMeshSurface.AddData();
    }

    NavMeshData InitializeBakeData()
    {
        var emptySources = new List<NavMeshBuildSource>();
        var emptyBounds = new Bounds();
        return NavMeshBuilder.BuildNavMeshData(navMeshSurface.GetBuildSettings(), emptySources, emptyBounds,
            navMeshSurface.transform.position, navMeshSurface.transform.rotation);
    }
    
    private void OnDestroy()
    {
        navMeshSurface.RemoveData();
    }
}
