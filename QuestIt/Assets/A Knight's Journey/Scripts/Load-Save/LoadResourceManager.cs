using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public static class LoadResourceManager
{
    public static PlayerInfo GetInfoFromResources(string path)
    {
        TextAsset asset = Resources.Load<TextAsset>(path);

        PlayerInfo info = Json.Deserialize(asset.ToString()) as PlayerInfo;

        return info;
    }
}
