using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonManager
{
    public static string DATA_FILE_PATH => Application.dataPath + "/dialogue.json";
    public static string SAVEDATA_FILE_PATH => Application.dataPath + "/data.json";

    private static JsonData _loadedData;
    public static JsonData loadedData
    {
        get
        {
            if (_loadedData == null)
            {
                MakeSureDataFileExists(DATA_FILE_PATH);
                string json = File.ReadAllText(DATA_FILE_PATH);
                JsonData saveData = JsonConvert.DeserializeObject<JsonData>(json);
                if (saveData == null)
                {
                    saveData = new JsonData();
                    saveData.dialogues = new Dictionary<string, NewDialogue>();
                    saveData.allImages = new List<NewImage>();
                    saveData.islandImageByHealth = new Dictionary<float, string>();
                    saveData.gameEndConditions = new Dictionary<float, string>();
                    saveData.gameEndConditions.Add(30, "humans_win");
                    saveData.gameEndConditions.Add(70, "neutral");
                    saveData.gameEndConditions.Add(100, "island_win");
                }

                _loadedData = saveData;
                MonoBehaviour.print("Game Loaded " + saveData.dialogues.Count + " dialogues.");
            }
            return _loadedData;
        }
    }


    public static void SaveGame(Dictionary<float, string> gameEndConditions, Dictionary<string, NewDialogue> allDialogues, Dictionary<float, string> bgByLife, List<NewImage> allImages)
    {
        JsonData saveData = new JsonData();
        saveData.dialogues = new Dictionary<string, NewDialogue>(allDialogues);
        saveData.allImages = new List<NewImage>(allImages);
        saveData.gameEndConditions = new Dictionary<float, string>(gameEndConditions);
        saveData.islandImageByHealth = new Dictionary<float, string>(bgByLife);

        string json = JsonConvert.SerializeObject(saveData);

        MakeSureDataFileExists(SAVEDATA_FILE_PATH);
        File.WriteAllText(SAVEDATA_FILE_PATH, json);

        // _loadedData = saveData;
    }


    public static void MakeSureDataFileExists(string fileName)
    {
        if (!File.Exists(fileName))
        {
            File.AppendAllText(fileName, "");
        }
    }
}

public class JsonData
{
    public Dictionary<float, string> gameEndConditions;
    public Dictionary<float, string> islandImageByHealth;
    public Dictionary<string, NewDialogue> dialogues;
    public List<NewImage> allImages;
}
