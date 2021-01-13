using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlacing : MonoBehaviour
{
    [SerializeField] private SpawnSettings spawnSettings;

    private readonly List<Vector2> vacantPositions = new List<Vector2>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        var unitSize = spawnSettings.spawnUnitSize;
        for (var row = spawnSettings.minGameY; row < spawnSettings.maxGameY; row += unitSize)
        {
            for (var col = spawnSettings.minGameX; col < spawnSettings.maxGameX; col += unitSize)
            {
                var currentPosition = new Vector2(col, row);
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
        return vacantPositions.Remove(FindUnitSizeRounded(new Vector2(position.x, position.y)));
    }

    public bool AddPosition(Vector2 position)
    {
        var toAdd = FindUnitSizeRounded(new Vector2(position.x, position.y));
        if (IsVacant(toAdd))
        {
            vacantPositions.Add(toAdd);
            return true;
        }

        return false;
    }

    private bool IsInDeadZone(Vector2 position)
    {
        if (!spawnSettings.useDeadZone) return false;
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

    private Vector2 FindUnitSizeRounded(Vector2 position)
    {
        var minX = spawnSettings.minGameX;
        var minY = spawnSettings.minGameY;
        var unitSize = spawnSettings.spawnUnitSize;
        var xResult = minX + Mathf.Round((Mathf.Abs(position.x - minX)) / unitSize) * unitSize;
        var yResult = minY + Mathf.Round((Mathf.Abs(position.y - minY)) / unitSize) * unitSize;
        return new Vector2(xResult, yResult);
    }

    private Vector2 GetRandomPositionInSquare(Vector2 position)
    {
        var xResult = UnityEngine.Random.Range(position.x, position.x + spawnSettings.spawnUnitSize);
        var yResult = UnityEngine.Random.Range(position.y, position.y + spawnSettings.spawnUnitSize);
        return new Vector2(xResult, yResult);
    }
}