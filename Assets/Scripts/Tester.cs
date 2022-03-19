using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Audio;
using Game;
using Newtonsoft.Json;
using TileDataIO;
using Tiles;
using Unitilities.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Debug = UnityEngine.Debug;

public class Tester : MonoBehaviour
{
    [Serializable]
    public class LinkNode
    {
        [JsonProperty(IsReference = true)]
        public LinkNode next;
        public int val;

        public LinkNode(int val = 0, LinkNode next = null)
        {
            this.val = val;
            this.next = next;
        }
    }


    public GameObject grid;
    public Tilemap map;
    public TileDataMgr dataMgr;

    public AudioClip clip;
    public GridCell cellTemplate;
    public GridCell emptyGround;
    public ObjectRefTable objectRefTable;
    public LinkNode node;
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
        List<LinkNode> linkList = new List<LinkNode>();
        int count = 4;
        for (int i = 0; i < count; i++)
        {
            if(i>0)
            {
                linkList.Add(new LinkNode(i.GetHashCode()));
                linkList[i - 1].next = linkList[i];
            }else if(i==0)
            {
                linkList.Add(new LinkNode(i.GetHashCode()));
            }
            if (i == count - 1)
            {
                linkList[i].next = linkList[0];
            }
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
            {ReferenceLoopHandling = ReferenceLoopHandling.Serialize};
        var s = JsonConvert.SerializeObject(linkList[0], Formatting.Indented, settings);
        Debug.Log(s);
        node = JsonConvert.DeserializeObject<LinkNode>(s);
        StringBuilder temp = new StringBuilder();
        for (int i = 0; i < count; i++)
        {
            temp.AppendLine(node.val.ToString());
            node = node.next;
        }
        Debug.Log(temp);
    }

    [ContextMenu("Test Play Sound")]
    public void TestSound()
    {
        _audioManager.PlaySound(_audioManager.GetTrackIDFromName("BackGround"), clip);
    }
}
