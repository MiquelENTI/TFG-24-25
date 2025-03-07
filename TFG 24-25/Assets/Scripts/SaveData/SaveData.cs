using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveData: Singleton<SaveData>
{
    ReplayData data;
    int match;

    SaveData()
    {
        this.data = new ReplayData(); 
        this.match = -1;
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + "/save" + match + ".dat";
        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);
        fileStream.Close();
    }

    public void SaveNewAction(string newAction)
    {
        data.inputs.Enqueue(newAction);
    }

    public void SaveDeck(Queue<int> deckSave)
    {
        data.deck = deckSave;
    }

    public ReplayData LoadReplay(int match)
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
