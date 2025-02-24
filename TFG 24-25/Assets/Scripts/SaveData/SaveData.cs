using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveData
{
    public static void Save(Queue<string> actions, int match)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/save"+ match +".dat";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        ReplayData data = new ReplayData(actions);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public static ReplayData LoadReplay(int match)
    {
        string path = Application.dataPath + "/save" + match + ".dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ReplayData data = formatter.Deserialize(fileStream) as ReplayData;
            fileStream.Close();

            return data;
        }
        else
        {
            return null;
        }
        
    }
}
