using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonManager
{
    public static string DATA_FILE_PATH => Application.dataPath + "/data.json";

    private static JsonData _loadedData;
    public static JsonData loadedData
    {
        get
        {
            if (_loadedData == null)
            {
                MakeSureDataFileExists();
                string json = File.ReadAllText(DATA_FILE_PATH);
                JsonData saveData = JsonConvert.DeserializeObject<JsonData>(json);
                if (saveData == null)
                {
                    saveData = new JsonData();
                    saveData.dialogues = new Dictionary<string, NewDialogue>();
                    saveData.allImages = new List<NewImage>();
                }

                _loadedData = saveData;
                MonoBehaviour.print("Game Loaded " + saveData.dialogues.Count + " dialogues.");
            }
            return _loadedData;
        }
    }


    public static void SaveGame(Dictionary<string, NewDialogue> allDialogues, List<NewImage> allImages)
    {
        JsonData saveData = new JsonData();
        saveData.dialogues = new Dictionary<string, NewDialogue>(allDialogues);
        saveData.allImages = new List<NewImage>(allImages);

        string json = JsonConvert.SerializeObject(saveData);

        MakeSureDataFileExists();
        File.WriteAllText(DATA_FILE_PATH, json);

        _loadedData = saveData;
    }


    public static void MakeSureDataFileExists()
    {
        if (!File.Exists(DATA_FILE_PATH))
        {
            File.AppendAllText(DATA_FILE_PATH, "");
        }
    }
}

public class JsonData
{
    public Dictionary<string, NewDialogue> dialogues;
    public List<NewImage> allImages;
}
