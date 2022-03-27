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
        public string Author;

        public static MapInfo CreateInfo() => new MapInfo
        {
            tileDataDirectory = "./",
            entityDataPath = "./Entities.json",
            createdTime = DateTime.Now,
            lastEditedTime = DateTime.Now,
            Author = "Default"
        };

        [UsedImplicitly]
        public bool ShouldSerializeAuthor()
        {
            if (string.IsNullOrEmpty(Author)) return false;
            if (Author == "Default") return false;
            return true;
        }
    }
}
