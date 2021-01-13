using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDisplay : MonoBehaviour
{
    public Sprite[] wallsSprites;
    private SpriteRenderer renderer;
    
    public int curLevel;

    private void Start()
    {
        curLevel = 0;
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Upgrade(object arg)
    {
        if ((GameObject)arg == gameObject)
        {
            curLevel++;
            renderer.sprite = wallsSprites[curLevel];
        }
    }

}
