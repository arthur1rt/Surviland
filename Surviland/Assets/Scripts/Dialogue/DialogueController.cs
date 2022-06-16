using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public Dictionary<string, NextDialogue> allDialogues;
    public GameObject[] allOptionObjects;

    private string dialogueDisplaying = "";
    private string dialoguePath = "START";

    private Dialogue dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        allDialogues = JsonManager.loadedData.dialogues;
        Dictionary<string, string> allPaths = new Dictionary<string, string>();
        foreach (string diagKey in allDialogues.Keys)
        {
            if (allPaths.ContainsValue(diagKey))
            {
                Debug.LogError("Duplicate dialogue key found: " + diagKey);
            }

            NextDialogue diag = allDialogues[diagKey];

            foreach (string path in diag._pathTillHere)
            {
                if (allPaths.ContainsKey(path))
                {
                    Debug.LogError("Duplicate dialogue path found: " + path);
                }
                allPaths.Add(path, diagKey);
            }
        }


        dialogueBox = transform.GetComponentInChildren<Dialogue>();

        HideAllOptions();


        Dictionary<string, NextDialogue> allDiag = new Dictionary<string, NextDialogue>();
        List<string> texts = new List<string> { "Hey, i'm a first dialogue, can u believe it?", "Yeah, that's super cool. I am a second dialogue :)" };
        List<string> options = new List<string> { "Option 1", "Option 2" };
        List<string> paths = new List<string> { "START" };
        NextDialogue dd = new NextDialogue(paths, texts, options, 0.05f, "ffffff");
        allDiag.Add("aa", dd);

        texts = new List<string> { "DDDD 1 LOL ", "D2 dialogue 2", "Dialogue 3 lol hahaha" };
        options = new List<string> { "Option 11", "Option 22", "Option 33", "Option 44" };
        paths = new List<string> { "aa1" };
        dd = new NextDialogue(paths, texts, options, 0.05f, "ff2929");
        allDiag.Add("bb", dd);


        texts = new List<string> { "another option of text" };
        options = new List<string> { "Option 11", "Option 22" };
        paths = new List<string> { "bb1", "bb3" };
        dd = new NextDialogue(paths, texts, options, 0.05f, "ffffff");
        allDiag.Add("cc", dd);

        texts = new List<string> { "again, different one", "this one has two texts lol" };
        options = new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" };
        paths = new List<string> { "bb2", "bb4" };
        dd = new NextDialogue(paths, texts, options, 0.05f, "ffffff");
        allDiag.Add("dd", dd);


        JsonManager.SaveGame(allDiag);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowDialogue(string dialogueKey)
    {
        if (allDialogues.ContainsKey(dialogueKey) == false) return;

        dialogueDisplaying = dialogueKey;
        dialogueBox.InitializeDialogue(allDialogues[dialogueKey]._allText);
    }

    public bool ShowOptionsForCurrentDialogue()
    {
        if (allDialogues.ContainsKey(dialogueDisplaying) == false) return false;

        List<string> optionsText = allDialogues[dialogueDisplaying]._options;

        List<GameObject> optionsToReveal = new List<GameObject>();
        if (optionsText.Count == 2)
        {
            optionsToReveal.Add(allOptionObjects[1]);
            optionsToReveal.Add(allOptionObjects[2]);
        }
        else
        {
            optionsToReveal = new List<GameObject>(allOptionObjects);
        }

        for (int i = 0; i < allOptionObjects.Length; i++)
        {
            GameObject option = allOptionObjects[i];
            if (optionsToReveal.Contains(option) == false)
            {
                HideOption(option);
            }
            else
            {
                optionsToReveal[i].SetActive(true);
                optionsToReveal[i].GetComponentInChildren<Text>().text = optionsText[i];
            }
        }
        return true;
    }

    public void HideAllOptions()
    {
        foreach (GameObject option in allOptionObjects)
        {
            HideOption(option);
        }
    }

    private void HideOption(GameObject option)
    {
        option.GetComponentInChildren<Text>().text = "";
        option.SetActive(false);
    }
}

public class NextDialogue
{
    public List<string> _pathTillHere;
    public List<string> _allText;
    public List<string> _options;
    public float _speed;
    public string _textColor;

    public NextDialogue(List<string> pathTillHere, List<string> allText, List<string> options, float speed, string textColor)
    {
        _pathTillHere = pathTillHere;
        _allText = allText;
        _speed = speed;
        _textColor = textColor;
        _options = options;
    }
}