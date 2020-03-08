using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using UnityEngine;
using MiniJSON;
using System.Text;

public static class SaveManager
{
    public static bool SavePathJson(PlayerInfo info, string path)
    {
        bool success = true;

        try
        {
            string json = Json.Serialize(info);

            using (FileStream fileStream = File.Open(path,FileMode.OpenOrCreate))
            {
                byte[] streamData = Encoding.ASCII.GetBytes(json);

                fileStream.Write(streamData, 0, streamData.Length);
            }
        }
        catch (Exception e)
        {
            success = false;
        }

        return success;
    }

    public static bool SaveResourceJson(PlayerInfo info,string name)
    {
        bool success = true;

        try
        {
            string json = JsonUtility.ToJson(info, true);

            Logger.Error(json);

            string path = Path.Combine(UnityEngine.Application.persistentDataPath, name + ".json");

            using (FileStream fs = File.Open(path,FileMode.OpenOrCreate))
            {
                byte[] streamData = Encoding.ASCII.GetBytes(json);

                fs.Write(streamData, 0, streamData.Length);
            }
        }
        catch(Exception e)
        {
            success = false;

            Logger.Error(e.Message);
        }

        return success;
    }
}
