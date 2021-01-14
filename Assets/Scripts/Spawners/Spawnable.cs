using UnityEngine;

public abstract class Spawnable : MonoBehaviour
{

    private Spawner spawner;

    internal void Spawn(Spawner spawner)
    {
        this.spawner = spawner;
    }

    protected virtual void OnDisable()
    {
        spawner?.SpawnableDeath(this);
    }
}
