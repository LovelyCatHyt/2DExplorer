using System;
using System.IO;
using Audio;
using Game;
using Newtonsoft.Json;
using Settings;
using TileDataIO;
using Tiles;
using Ui.Prototype;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Debug = UnityEngine.Debug;

public class Tester : MonoBehaviour
{

    public GameObject grid;
    public Tilemap map;
    public TileDataMgr dataMgr;

    public AudioClip clip;
    public GridCell cellTemplate;
    public GridCell emptyGround;
    public ObjectRefTable objectRefTable;
    public string testRelativePath;
    [Space]
    public GameObject uiPrefab;
    public ListView listView;
    [Inject] private AudioManager _audioManager;
    [Inject] private TilemapManager _tilemapManager;
    [Inject] private GameInstance _gameInstance;
    [Inject] private SettingsManager _settingsManager;

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

    [ContextMenu("Test: Add prefab list view")]
    public void Test()
    {
        listView.Add(uiPrefab);
    }

    [ContextMenu("Test Play Sound")]
    public void TestSound()
    {
        _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), clip);
    }
}
