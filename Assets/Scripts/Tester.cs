using System.Collections.Generic;
using Audio;
using Game;
using Newtonsoft.Json;
using TileDataIO;
using Tiles;
using Unitilities;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Object = UnityEngine.Object;

public class Tester : MonoBehaviour
{
    public GameObject grid;
    public Tilemap map;
    public TileDataMgr dataMgr;

    public AudioClip clip;
    public GridCell cellTemplate;
    public GridCell emptyGround;
    public ObjectRefTable objectRefTable;
    [Inject] private AudioManager _audioManager;
    [Inject] private TilemapManager _tilemapManager;
    [Inject] private GameInstance _gameInstance;

    [Inject]
    public void Init()
    {
        var events = _gameInstance.events;
        events.onGameStart.AddListener(GameStart);
        events.onBeforeSave.AddListener(BeforeSaving);
        events.onSaveFinished.AddListener(SavedFinished);
        events.onBeforePause.AddListener(Pause);
        events.onPauseResumed.AddListener(Resume);
    }

    private void Resume()
    {
        Debug.Log($"Resumed! Time.fixedDeltaTime = {Time.fixedDeltaTime}, deltaTime = {Time.deltaTime}");
    }

    private void Pause()
    {
        Debug.Log($"Paused! Time.fixedDeltaTime = {Time.fixedDeltaTime}, deltaTime = {Time.deltaTime}");
    }

    private void GameStart()
    {
        Debug.Log("Game start!");
    }

    private void BeforeSaving()
    {
        Debug.Log("InGame->Saving");
    }

    private void SavedFinished()
    {
        Debug.Log("Saving->InGame");
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var pos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    _tilemapManager.SetCell(pos, cellTemplate);
        //}

        if (Input.GetMouseButtonDown(1))
        {
            var pos = map.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            _tilemapManager.SetCell(pos, emptyGround);
        }
    }

    [ContextMenu("Test")]
    public void Test()
    {
        HashSet<int> test = new HashSet<int>();
        test.Add(1);
        test.Add(1);
        Debug.Log(test.Contains(1));
    }

    [ContextMenu("Test Play Sound")]
    public void TestSound()
    {
        _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), clip);
    }

    //[ContextMenu("Test Save")]
    //public void TestSave()
    //{
    //    dataMgr.SaveWholeGrid(grid);
    //}

    //[ContextMenu("Test Load")]
    //public void TestLoad()
    //{
    //    dataMgr.LoadWholeGrid(grid);
    //}
}
