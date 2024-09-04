using System;
using System.Collections.Generic;
using EntitySchemas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace WebsocketMessage
{
    public class WsMessage
    {
        [JsonProperty(PropertyName = "detail")]
        public JObject Detail = new();

        [JsonProperty(PropertyName = "type")]
        public string Type = "";
    }
}

namespace Session
{
    public class Session
    {
        [JsonProperty(PropertyName = "id")]
        public string ID;

        [JsonProperty(PropertyName = "name")]
        public string Name;
    }

    public class CreateSession
    {
        [JsonProperty(PropertyName = "name")]
        public string Name;

        [JsonProperty(PropertyName = "password")]
        public string Password;
    }

    public class JoinSession : CreateSession
    {
    }
}

namespace EntitySchemas
{
    public class EntityData
    {
        [JsonProperty(PropertyName = "cells")]
        public List<int> Cells;

        [JsonProperty(PropertyName = "direction")]
        public int Direction;

        [JsonProperty(PropertyName = "size")]
        public int Size;
    }

    public class EntitiesDict
    {
        [JsonProperty(PropertyName = "entities")]
        public Dictionary<string, EntityData> Entities;
    }
}

namespace Player
{
    public class PlayerID
    {
        [JsonProperty(PropertyName = "playerId")]
        public string ID;
    }

    public class PlayerPlacement
    {
        [JsonProperty(PropertyName = "board")]
        public string Board;

        [JsonProperty(PropertyName = "entities")]
        public Dictionary<string, EntityData> EntitiesDict { get; set; } = new();
    }

    public class Hit
    {
        [JsonProperty(PropertyName = "cell")]
        public int Cell;

        [JsonProperty(PropertyName = "entityId")]
        public string EntityID;
    }

    public class HitResponse
    {
        [JsonProperty(PropertyName = "cell")]
        public int Cell;

        [JsonProperty(PropertyName = "entityId")]
        public string EntityID;

        [JsonProperty(PropertyName = "status")]
        public string Status;
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