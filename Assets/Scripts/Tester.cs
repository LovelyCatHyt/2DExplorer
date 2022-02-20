using System;
using System.Collections;
using System.Collections.Generic;
using TileDataIO;
using Tiles;
using TileTool;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tester : MonoBehaviour
{
    public GameObject grid;
    public Tilemap map;
    public TileDataMgr dataMgr;

    [ContextMenu("Test Save")]
    public void TestSave()
    {
        dataMgr.SaveWholeGrid(grid);
    }

    [ContextMenu("Test Load")]
    public void TestLoad()
    {
        dataMgr.LoadWholeGrid(grid);
    }
}
