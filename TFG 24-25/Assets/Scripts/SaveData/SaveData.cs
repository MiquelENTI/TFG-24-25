using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveData: Singleton<SaveData>
{
    ReplayData data;

    public string pathFolder = "/save";

    SaveData()
    {
        data = new ReplayData(); 
    }

    public void Save()
    {

        bool fileExits = true;
        int nFile = 0;

        while (fileExits)
        {
            fileExits = SaveData.Instance.CheckFileExists(nFile);

            if (fileExits)
                nFile++;
        }


        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.dataPath + pathFolder + nFile + ".dat";
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
        data.deck = new Queue<int>(deckSave);
    }

    public ReplayData LoadReplay(int match)
    {
        string path = Application.dataPath + pathFolder + match + ".dat";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);

            ReplayData replayData = formatter.Deserialize(fileStream) as ReplayData;
            fileStream.Close();

            return replayData;
        }
        else
        {

            Debug.LogError("Replay does not exist or path error");
            return null;
        }
    }

    private bool CheckFileExists(int match)
    {
        string path = Application.dataPath + pathFolder + match + ".dat";
        return File.Exists(path);
    }
}
