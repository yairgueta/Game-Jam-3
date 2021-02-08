using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Sprite nodeSprite;
    [SerializeField] private Tile unwalkable, walkable;
    [SerializeField] private List<GameObject> objectsToScan;

    private Grid grid;


    private void Awake()
    {
        grid = GetComponent<Grid>();
        if (Application.isPlaying)
        {
            var temp = Instantiate(tilemap, tilemap.transform.parent);
            Destroy(tilemap.gameObject);
            tilemap = temp;
            ScanAll();
        }
        
        
    }
    
    void ScanAll()
    {
        
        foreach (var obj in objectsToScan)
        {
            ScanOne(obj);
        }
    }

    void ScanOne(GameObject go)
    {
        var col = go.GetComponent<Collider2D>();
        if (col) Draw(col.bounds);
        foreach (Transform child in go.transform)
        {
            ScanOne(child.gameObject);
        }
    }

    void Draw(Bounds bnds)
    {
        var min = grid.WorldToCell(bnds.min);
        var max = grid.WorldToCell(bnds.max);
        tilemap.BoxFill(min, unwalkable, min.x, min.y, max.x, max.y);
    }

    
    
#if UNITY_EDITOR
    private void Update()
    {
        if (Application.isPlaying) return;
        grid ??= tilemap.layoutGrid;
        var tileMapSprite = Sprite.Create(nodeSprite.texture, 
            new Rect(0, 0, nodeSprite.texture.width, nodeSprite.texture.height), 
            new Vector2(.5f, .5f), 
            nodeSprite.pixelsPerUnit / grid.cellSize.x);
        unwalkable.sprite = tileMapSprite;
        walkable.sprite = tileMapSprite;
        tilemap.RefreshAllTiles();
    }
#endif
    
}
