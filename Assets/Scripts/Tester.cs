using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Newtonsoft.Json;
using TileDataIO;
using Tiles;
using TileTool;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Util.PropAttr;
using Zenject;

public class Tester : MonoBehaviour
{
    public GameObject grid;
    public Tilemap map;
    public TileDataMgr dataMgr;

    public AudioClip clip;
    public GridCell cellTemplate;
    public GridCell emptyGround;
    [Inject] private AudioManager _audioManager;
    [Inject] private TilemapManager _tilemapManager;

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var pos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    _tilemapManager.SetCell(pos, cellTemplate);
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    var pos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    _tilemapManager.SetCell(pos, emptyGround);
        //}
    }

    [ContextMenu("Test Play Sound")]
    public void TestSound()
    {
        _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), clip);
    }

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
