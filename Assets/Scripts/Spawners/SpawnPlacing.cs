using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlacing : MonoBehaviour
{
    [SerializeField] private SpawnSettings spawnSettings;

    private List<Vector2> vacantPositions = new List<Vector2>();

    public void Initialize()
    {
        var unitSize = spawnSettings.spawnUnitSize;
        for (float row = spawnSettings.minGameY; row < spawnSettings.maxGameY; row += unitSize)
        {
            for (float col = spawnSettings.minGameX; col < spawnSettings.maxGameX; col += unitSize)
            {
                Vector2 currentPosition = new Vector2(col, row);
                if (IsVacant(currentPosition) && !IsInDeadZone(currentPosition))
                {
                    vacantPositions.Add(currentPosition);
                }
            }
        }
    }

    public Vector2 GetVacantPosition()
    {
        var index = UnityEngine.Random.Range(0, vacantPositions.Count);
        return GetRandomPositionInSquare(vacantPositions[index]);
    }

    public bool RemovePosition(Vector2 position)
    {
        return vacantPositions.Remove(new Vector2(FindUnitSizeRoundedForX(position.x),
            FindUnitSizeRoundedForY(position.y)));
    }

    public bool AddPosition(Vector2 position)
    {
        var toAdd = new Vector2(FindUnitSizeRoundedForX(position.x), FindUnitSizeRoundedForY(position.y));
        if (IsVacant(toAdd))
        {
            vacantPositions.Add(toAdd);
            return true;
        }

        return false;
    }

    private bool IsInDeadZone(Vector2 position)
    {
        return spawnSettings.deadZoneMinX < position.x && position.x < spawnSettings.deadZoneMaxX &&
               position.y > spawnSettings.deadZoneMinY && position.y < spawnSettings.deadZoneMaxY;
    }

    private bool IsVacant(Vector2 position)
    {
        var unitSize = spawnSettings.spawnUnitSize;
        RaycastHit2D boxCollision =
            Physics2D.BoxCast(position, new Vector2(unitSize, unitSize),
                0f, Vector2.zero);
        return boxCollision.collider == null;
    }

    public float FindUnitSizeRoundedForX(float num)
    {
        var minX = spawnSettings.minGameX;
        var unitSize = spawnSettings.spawnUnitSize;
        return minX + Mathf.Round((Mathf.Abs(num - minX)) / unitSize) * unitSize;
    }
    
    
    public float FindUnitSizeRoundedForY(float num)
    {
        var minY = spawnSettings.minGameY;
        var unitSize = spawnSettings.spawnUnitSize;
        return minY + Mathf.Round((Mathf.Abs(num - minY)) / unitSize) * unitSize;
    }
    
    private Vector2 GetRandomPositionInSquare(Vector2 position)
    {
        var xResult = UnityEngine.Random.Range(position.x, position.x + spawnSettings.spawnUnitSize);
        var yResult = UnityEngine.Random.Range(position.y, position.y + spawnSettings.spawnUnitSize);
        return new Vector2(xResult, yResult);
    }
    
}
