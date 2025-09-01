using System;
using System.Collections.Generic;

namespace Theymes
{
    class TheymesJsonHelpers {
        public static SimpleJSON.JSONObject PlayerToJsonObject(TheymesPlayer player)
        {
            if (player == null)
            {
                return null;
            }

            var playerObject = new SimpleJSON.JSONObject();
            if (player.id != null) playerObject["id"] = player.id;
            if (player.name != null) playerObject["name"] = player.name;
            if (player.email != null) playerObject["email"] = player.email;
            if (player.tier != null) playerObject["tier"] = player.tier;

            return playerObject;
        }

        public static string PlayerToJson(TheymesPlayer player)
        {
            SimpleJSON.JSONObject playerJson = PlayerToJsonObject(player);

            if (playerJson == null)
            {
                return null;
            }

            return playerJson.ToString();
        }

        public static TheymesPlayer JsonToPlayer(string jsonStr)
        {
            if (jsonStr == null)
            {
                return null;
            }

            var json = SimpleJSON.JSON.Parse(jsonStr);
            if (json == null || !json.IsObject)
            {
                return null;
            }

            var player = new TheymesPlayer();

            if (json["id"] != null && json["id"].IsString) player.id = json["id"];
            if (json["name"] != null && json["name"].IsString) player.name = json["name"];
            if (json["email"] != null && json["email"].IsString) player.email = json["email"];
            if (json["tier"] != null && json["tier"].IsNumber) player.tier = json["tier"].AsInt;

            return player;
        }

        public static SimpleJSON.JSONObject ConfigToJsonObject(TheymesConfig config)
        {
            if (config == null)
            {
                return null;
            }

            // Create the main JSON object
            var jsonNode = new SimpleJSON.JSONObject();

            // Set the language if it's not null
            if (config.language != null)
            {
                jsonNode["language"] = config.language;
            }

            // Set the signedMetadataToken if it's not null
            if (config.signedMetadataToken != null)
            {
                jsonNode["signedMetadataToken"] = config.signedMetadataToken;
            }

            // Add tags array
            if (config.tags != null)
            {
                jsonNode["tags"] = StringListToJsonArray(config.tags);
            }

            if (config.fields != null)
            {
                jsonNode["fields"] = DictionaryToJsonObject(config.fields);
            }

            // Add player object if it exists
            if (config.player != null)
            {
                jsonNode["player"] = PlayerToJsonObject(config.player);
            }

            return jsonNode;
        }

        public static string ConfigToJson(TheymesConfig config)
        {
            SimpleJSON.JSONObject configJson = ConfigToJsonObject(config);

            if (configJson == null)
            {
                return null;
            }

            return configJson.ToString();
        }

        public static List<string> JsonToStringList(string jsonStr)
        {
            if (jsonStr == null)
            {
                return null;
            }

            var json = SimpleJSON.JSON.Parse(jsonStr);
            if (json == null || !json.IsArray)
            {
                return null;
            }

            var list = new List<string>();
            foreach (var kvp in json.AsArray)
            {
                var item = kvp.Value;
                if (item.IsString)
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }

        public static SimpleJSON.JSONArray StringListToJsonArray(List<string> list)
        {
            if (list == null)
            {
                return null;
            }

            var jsonArray = new SimpleJSON.JSONArray();
            foreach (var item in list)
            {
                jsonArray.Add(item);
            }

            return jsonArray;
        }

        public static string StringListToJson(List<string> list)
        {
            SimpleJSON.JSONArray jsonArray = StringListToJsonArray(list);
            if (jsonArray == null)
            {
                return null;
            }

            return jsonArray.ToString();
        }

        public static SimpleJSON.JSONNode ObjectToJsonNode(object value)
        {
            if (value is string str)
                return new SimpleJSON.JSONString(str);
            else if (value is double || value is float || value is int)
                return new SimpleJSON.JSONNumber(Convert.ToDouble(value));
            else if (value is bool b)
                return new SimpleJSON.JSONBool(b);
            else
                return SimpleJSON.JSONNull.CreateOrGet();
        }

        public static string ObjectToJson(object value)
        {
            return ObjectToJsonNode(value).ToString();
        }

        public static object JsonNodeToObject(SimpleJSON.JSONNode value)
        {
            if (value.IsString)
                return value.Value;
            else if (value.IsNumber)
                return value.AsDouble;
            else if (value.IsBoolean)
                return value.AsBool;
            else if (value.IsNull)
                return null;
            else if (value.IsArray)
            {
                var list = new List<object>();
                foreach (var kvp in value.AsArray)
                {
                    list.Add(JsonNodeToObject(kvp.Value));
                }
                return list;
            }
            return null;
        }

        public static Dictionary<string, object> JsonToDictionary(string jsonStr)
        {
            if (jsonStr == null)
            {
                return null;
            }

            var json = SimpleJSON.JSON.Parse(jsonStr);
            if (json == null || !json.IsObject)
            {
                return null;
            }

            var dict = new Dictionary<string, object>();
            foreach (var kvp in json.AsObject)
            {
                var value = kvp.Value;
                if (value.IsString)
                    dict[kvp.Key] = value.Value;
                else if (value.IsNumber)
                    dict[kvp.Key] = value.AsDouble;
                else if (value.IsBoolean)
                    dict[kvp.Key] = value.AsBool;
                else if (value.IsNull)
                    dict[kvp.Key] = null;
                else if (value.IsArray)
                    dict[kvp.Key] = null;
            }

            return dict;
        }

        public static SimpleJSON.JSONObject DictionaryToJsonObject(Dictionary<string, object> dict)
        {
            if (dict == null)
            {
                return null;
            }

            var jsonObj = new SimpleJSON.JSONObject();
            foreach (var kvp in dict)
            {
                jsonObj[kvp.Key] = ObjectToJsonNode(kvp.Value);
            }

            return jsonObj;
        }

        public static string DictionaryToJson(Dictionary<string, object> dict)
        {
            SimpleJSON.JSONObject jsonObj = DictionaryToJsonObject(dict);
            if (jsonObj == null)
            {
                return null;
            }

            return jsonObj.ToString();
        }
    }
}
