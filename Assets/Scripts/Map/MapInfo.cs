using System;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace TileDataIO
{
    public struct MapInfo
    {
        [JsonProperty("TileDataDirectory")]
        public string tileDataDirectory;
        [JsonProperty("EntityDataPath")]
        public string entityDataPath;
        [JsonProperty("CreatedTime")]
        public DateTime createdTime;
        [JsonProperty("LastEditedTime")]
        public DateTime lastEditedTime;
        [JsonProperty("Author")]
        public string author;
        [JsonProperty("DisplayName")]
        public string displayName;

        public static MapInfo CreateInfo() => new MapInfo
        {
            tileDataDirectory = "./",
            entityDataPath = "./Entities.json",
            createdTime = DateTime.Now,
            lastEditedTime = DateTime.Now,
            author = "Default",
            displayName = "默认地图"
        };

        [UsedImplicitly]
        // ReSharper disable once IdentifierTypo
        public bool ShouldSerializeauthor()
        {
            if (string.IsNullOrEmpty(author)) return false;
            if (author == "Default") return false;
            return true;
        }
    }
}
