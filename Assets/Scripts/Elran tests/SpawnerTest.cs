using System.Collections;
using System.Collections.Generic;
using Spawners;
using UnityEngine;

public class SpawnerTest : MonoBehaviour
{
    // private SpawnPlacing _spawnPlacing;
    // [SerializeField] private Transform toCheck;
    private Spawner2 spawner;
    
    // Debug.DrawLine(new Vector3(currentPosition.x - 5f, currentPosition.y, 0f), new Vector3( currentPosition.x + 5f,  currentPosition.y, 0f), Color.green, 100f);
    // Debug.DrawLine(new Vector3( currentPosition.x,  currentPosition.y - 5f, 0f), new Vector3( currentPosition.x,  currentPosition.y +5f, 0f), Color.green, 100f);
    // Debug.Log(currentPosition);
    
    // Start is called before the first frame update
    void Start()
    {
        // _spawnPlacing = GetComponent<SpawnPlacing>();
        // _spawnPlacing.Initialize();
        spawner = GetComponent<Spawner2>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     var pos = _spawnPlacing.GetVacantPosition();
        //     Debug.Log(_spawnPlacing.GetVacantPosition());
        //     Debug.DrawLine(new Vector3(pos.x - 5f, pos.y, 0f), new Vector3(pos.x + 5f, pos.y, 0f), Color.green, 10f);
        //     Debug.DrawLine(new Vector3(pos.x, pos.y - 5f, 0f), new Vector3(pos.x, pos.y +5f, 0f), Color.green, 10f);
        // }
        //
        // if (Input.GetKey(KeyCode.A))
        // {
        //     var check = toCheck.position;
        //     Debug.Log(_spawnPlacing.RemovePosition(check));
        //     Debug.Log(_spawnPlacing.AddPosition(check));
        // }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spawner.SpawnGameObject();
        }
    }
}
