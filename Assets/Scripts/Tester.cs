using System;
using System.Collections;
using System.Collections.Generic;
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
    [Inject] private AudioManager _audioManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TestSound();
        }
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
