using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using TileTool;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tester : MonoBehaviour
{
    public Tilemap map;
    public TileBase clickToSet;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var cellPos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // Debug.Log($"mouse clicked at {cellPos}");
            var tile = map.GetTile(cellPos);

            //if (tile != clickToSet)
            //{
            //    map.SetTile(cellPos, clickToSet);
            //}
            //else
            //{
            //    //Debug.Log($"Try walk at {cellPos}");
            //    //int counter = 0;
            //    //TilemapWalker8.WalkNow(map, cellPos, clickToSet, (tilemap, i) =>
            //    //{
            //    //    Debug.Log($"<color=#88a0ff>[{counter}]</color>Visit at {i}");
            //    //    counter++;
            //    //});
            //}

        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            map.RefreshAllTiles();
        }
    }
}
