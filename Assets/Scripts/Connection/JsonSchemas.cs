using System;
using System.Collections.Generic;
using EntityJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace EntityJson
{
    public class Entities
    {
        [JsonProperty(PropertyName = "entities")]
        public Dictionary<string, EntityData> EntitiesDict { get; set; } = new();
    }

    public class EntityData
    {
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }

        [JsonProperty(PropertyName = "cells")]
        public List<int> Cells { get; set; }
    }
}

namespace RequestJson
{
    public class Request
    {
        [JsonProperty(PropertyName = "detail")]
        public JObject Detail = new();

        [JsonProperty(PropertyName = "player_id")]
        public string PlayerID = "";

        [JsonProperty(PropertyName = "type")]
        public string Type = "";
    }

    public class Ready : Entities
    {
        [JsonProperty(PropertyName = "board")]
        public string Board = "";
    }
}

namespace ResponseJson
{
    public class Response
    {
        [JsonProperty(PropertyName = "detail")]
        public JObject Detail = new();

        [JsonProperty(PropertyName = "type")]
        public string Type = "";
    }

    public class PlayerID
    {
        [JsonProperty(PropertyName = "player_id")]
        public string Id;
    }

    public class EnemyID
    {
        [JsonProperty(PropertyName = "enemy_id")]
        public string Id;
    }
}

namespace Serialize
{
    public static class jObject
    {
        public static T ToObject<T>(JObject jObject)
        {
            T obj = default;
            try
            {
                obj = jObject.ToObject<T>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserialization error: {e.Message}");
            }

            return obj;
        }

        public static JObject FromObject(object obj)
        {
            JObject jObj = default;
            try
            {
                jObj = JObject.FromObject(obj);
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserialization error: {e.Message}");
            }

            return jObj;
        }
    }
}