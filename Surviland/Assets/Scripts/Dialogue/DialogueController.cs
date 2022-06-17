using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public GameObject imagePrefab;
    private GameObject imagesFolder;


    public Dictionary<string, NewDialogue> allDialogues;
    public GameObject[] allOptionObjects;
    Dictionary<string, string> allDialoguePaths;

    public Dictionary<string, NewImage> allImagesById;

    private string dialogueDisplaying = "";
    private string dialoguePath = "START";

    private Dialogue dialogueBox;

    Dictionary<string, NewImage> imagesDisplaying;

    private GameController gameController;

    private bool gameIsOver = false;

    void Awake()
    {
        // Dictionary<string, NewDialogue> allDiag = new Dictionary<string, NewDialogue>();
        // List<string> texts = new List<string> { "Hey, i'm a first dialogue, can u believe it?", "Yeah, that's super cool. I am a second dialogue :)" };
        // List<string> options = new List<string> { "Option 1", "Option 2" };
        // List<string> paths = new List<string> { "START" };
        // Dictionary<string, bool> imgConfig = new Dictionary<string, bool>();
        // imgConfig.Add("island_healthy", true);
        // List<float> weights = new List<float> { -40, 0 };
        // NewDialogue dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("aa", dd);

        // texts = new List<string> { "DDDD 1 LOL ", "D2 dialogue 2", "Dialogue 3 lol hahaha" };
        // options = new List<string> { "Option 11", "Option 22", "Option 33", "Option 44" };
        // paths = new List<string> { "aa1" };
        // imgConfig = new Dictionary<string, bool>();
        // weights = new List<float> { -40, -10, 0, -25 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ff2929", imgConfig);
        // allDiag.Add("bb", dd);

        // texts = new List<string> { "another option of text" };
        // options = new List<string> { "Option 11", "Option 22" };
        // paths = new List<string> { "bb1", "bb3" };
        // imgConfig = new Dictionary<string, bool>();
        // weights = new List<float> { 20, -10 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("cc", dd);

        // texts = new List<string> { "again, different one", "this one has two texts lol" };
        // options = new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" };
        // paths = new List<string> { "bb2", "bb4" };
        // imgConfig = new Dictionary<string, bool>();
        // imgConfig.Add("img1", true);
        // imgConfig.Add("img2", true);
        // weights = new List<float> { 10, -10 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("dd", dd);



        // texts = new List<string> { "Ending 1 too bad" };
        // options = new List<string> { "Island win" };
        // paths = new List<string> { "island_win" };
        // imgConfig = new Dictionary<string, bool>();
        // weights = new List<float> { 0 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("island_win", dd);

        // texts = new List<string> { "Ending 2 too bad" };
        // options = new List<string> { "Neutral" };
        // paths = new List<string> { "neutral" };
        // imgConfig = new Dictionary<string, bool>();
        // weights = new List<float> { 0 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("neutral", dd);

        // texts = new List<string> { "Ending 3" };
        // options = new List<string> { "Humans win" };
        // paths = new List<string> { "humans_win" };
        // imgConfig = new Dictionary<string, bool>();
        // weights = new List<float> { 0 };
        // dd = new NewDialogue(paths, texts, options, weights, 0.02f, "ffffff", imgConfig);
        // allDiag.Add("humans_win", dd);



        // List<NewImage> imageRef = new List<NewImage>();
        // imageRef.Add(new NewImage("island_healthy", 0, 0, 0));
        // imageRef.Add(new NewImage("island_destroyed", 0, 0, 0));
        // imageRef.Add(new NewImage("img1", 500, 200, 1));
        // imageRef.Add(new NewImage("img2", -300, 100, 2));

        // Dictionary<float, string> endConditions = new Dictionary<float, string>();
        // endConditions.Add(30, "humans_win");
        // endConditions.Add(70, "neutral");
        // endConditions.Add(100, "island_win");

        // Dictionary<float, List<string>> imageByLife = new Dictionary<float, List<string>>();
        // imageByLife.Add(30, new List<string> { "island_destroyed", "image 2" });
        // imageByLife.Add(70, new List<string> { "island_healthy", "image 2", "image 3" });
        // imageByLife.Add(100, new List<string> { "island_healthy", "image 2" });
        // JsonManager.SaveGame(endConditions, allDiag, imageByLife, imageRef);
    }

    // Start is called before the first frame update
    void Start()
    {
        imagesFolder = transform.parent.Find("Images").gameObject;

        gameController = transform.GetComponent<GameController>();

        imagesDisplaying = new Dictionary<string, NewImage>();

        allDialogues = JsonManager.loadedData.dialogues;
        allDialoguePaths = new Dictionary<string, string>();

        foreach (string diagKey in allDialogues.Keys)
        {
            if (allDialoguePaths.ContainsValue(diagKey))
            {
                Debug.LogError("Duplicate dialogue key found: " + diagKey);
            }

            NewDialogue diag = allDialogues[diagKey];

            foreach (string path in diag._pathTillHere)
            {
                if (allDialoguePaths.ContainsKey(path))
                {
                    Debug.LogError("Duplicate dialogue path found: " + path);
                }
                allDialoguePaths.Add(path, diagKey);
            }
        }


        allImagesById = new Dictionary<string, NewImage>();
        foreach (NewImage img in JsonManager.loadedData.allImages)
        {
            allImagesById.Add(img._imageName, img);
            img.SetSprite(Resources.Load<Sprite>("Art/" + img._imageName));
        }


        dialogueBox = transform.GetComponentInChildren<Dialogue>();

        HideAllOptions();
        UpdateDialogue();
    }

    private void ShowDialogue(string dialogueKey)
    {
        if (allDialogues.ContainsKey(dialogueKey) == false) return;

        dialogueDisplaying = dialogueKey;
        dialogueBox.InitializeNewDialogue(allDialogues[dialogueKey]);
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
            for (int i = 0; i < optionsText.Count; i++)
            {
                optionsToReveal.Add(allOptionObjects[i]);
            }
        }

        int optionTextIndex = 0;
        for (int i = 0; i < allOptionObjects.Length; i++)
        {
            GameObject option = allOptionObjects[i];
            if (optionsToReveal.Contains(option) == false)
            {
                HideOption(option);
            }
            else
            {
                allOptionObjects[i].GetComponent<Button>().interactable = true;
                allOptionObjects[i].GetComponentInChildren<Text>().text = optionsText[optionTextIndex];
                optionTextIndex++;
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
        option.GetComponent<Button>().interactable = false;
    }

    private void UpdateDialogue()
    {
        if (allDialoguePaths.ContainsKey(dialoguePath) == false)
        {
            Debug.LogError("No dialogue was set up for the path " + dialoguePath);
            return;
        }
        dialogueDisplaying = allDialoguePaths[dialoguePath];
        dialogueBox.InitializeNewDialogue(allDialogues[dialogueDisplaying]);
        UpdateImages();
    }

    private void UpdateImages()
    {
        //refreshing images according to island life
        List<float> sortedBgConditions = GetSortedList(new List<float>(JsonManager.loadedData.islandImageByHealth.Keys));


        //remove all images
        foreach (float val in sortedBgConditions)
        {
            foreach (string imgToRemove in JsonManager.loadedData.islandImageByHealth[val])
            {
                if (imagesDisplaying.ContainsKey(imgToRemove))
                {
                    imagesDisplaying.Remove(imgToRemove);
                }
            }
        }

        string show = "";
        // add the images needed
        foreach (float val in sortedBgConditions)
        {
            if (gameController.life <= val)
            {
                foreach (string imgToAdd in JsonManager.loadedData.islandImageByHealth[val])
                {
                    if (imagesDisplaying.ContainsKey(imgToAdd) == false)
                    {
                        imagesDisplaying.Add(imgToAdd, allImagesById[imgToAdd]);
                        show += allImagesById[imgToAdd]._imageName + " / ";
                    }
                }
                break;
            }
        }

        // refresh images displaying to add and remove images
        if (allDialogues.ContainsKey(dialogueDisplaying))
        {
            // foreach (string imageKey in allDialogues[dialogueDisplaying]._imageConfig.Keys)
            // {
            //     if (imagesDisplaying.ContainsKey(imageKey))
            //     {
            //         if (allDialogues[dialogueDisplaying]._imageConfig[imageKey] == false)
            //         {
            //             imagesDisplaying.Remove(imageKey);
            //             continue;
            //         }
            //         else
            //         {
            //             imagesDisplaying[imageKey] = allImagesById[imageKey];
            //         }
            //     }
            //     else
            //     {
            //         if (allImagesById.ContainsKey(imageKey) == false)
            //         {
            //             Debug.LogError("Trying to display image not found: " + imageKey);
            //         }
            //         imagesDisplaying.Add(imageKey, allImagesById[imageKey]);
            //     }
            // }

            int imagesNeeded = imagesDisplaying.Count - imagesFolder.transform.childCount;
            // updates the number of images needed
            if (imagesNeeded < 0)
            {
                while (imagesNeeded < 0)
                {
                    Destroy(imagesFolder.transform.GetChild(imagesFolder.transform.childCount - 1).gameObject);
                    imagesNeeded++;
                }
            }
            else if (imagesNeeded > 0)
            {
                while (imagesNeeded > 0)
                {
                    GameObject newImg = Instantiate(imagePrefab);
                    newImg.transform.SetParent(imagesFolder.transform);
                    imagesNeeded--;
                }
            }


            // order all the images to be displayed
            List<NewImage> orderedImages = new List<NewImage>();
            while (orderedImages.Count < imagesDisplaying.Keys.Count)
            {
                NewImage smallerLayer = null;
                foreach (NewImage imageConfig in imagesDisplaying.Values)
                {
                    if (orderedImages.Contains(imageConfig)) continue;
                    if (smallerLayer == null || imageConfig._layer < smallerLayer._layer)
                    {
                        smallerLayer = imageConfig;
                    }
                }
                orderedImages.Add(smallerLayer);
            }


            for (int i = 0; i < orderedImages.Count; i++)
            {
                Transform child = imagesFolder.transform.GetChild(i);
                child.GetComponent<Image>().sprite = allImagesById[orderedImages[i]._imageName].GetSprite();
                child.GetComponent<RectTransform>().localPosition = new Vector2(orderedImages[i]._xPos, orderedImages[i]._yPos);
                if (orderedImages[i].GetSprite() == null)
                {
                    Debug.LogError("No image in project for " + orderedImages[i]._imageName);
                }
                child.GetComponent<RectTransform>().sizeDelta = orderedImages[i].GetSprite().rect.size;
                child.GetComponent<RectTransform>().localScale = Vector3.one;
            }
        }
    }

    public void ChooseOption(int number)
    {
        if (gameIsOver)
        {
            SceneManager.LoadScene("EndGame");
            return;
        }
        int optionIndex = -1;
        int skippedButtons = 0;
        for (int i = 0; i < allOptionObjects.Length; i++)
        {
            if ((i + 1) < number)
            {
                GameObject option = allOptionObjects[i];
                if (option.GetComponent<Button>().interactable) continue;
                skippedButtons++;
            }
            else
            {
                break;
            }
        }

        optionIndex = number - skippedButtons;
        try
        {
            gameController.ChangeLife(allDialogues[dialogueDisplaying]._optionsWeight[optionIndex - 1]);
            // print("Damage: " + allDialogues[dialogueDisplaying]._optionsWeight[optionIndex - 1]);
        }
        catch
        {
            Debug.LogError("Missing optionWeight value for dialogue " + dialogueDisplaying + " option " + optionIndex);
        }
        dialoguePath = dialogueDisplaying + optionIndex;
        dialogueBox.ClearAllDialogueBox();

        if (allDialoguePaths.ContainsKey(dialoguePath) == false)
        {
            print("Game over. No path has been found for " + dialoguePath);
            GameOver();
        }
        else
        {
            UpdateDialogue();
            HideAllOptions();
        }
    }


    private void GameOver()
    {
        List<float> sorted = GetSortedList(new List<float>(JsonManager.loadedData.gameEndConditions.Keys));

        foreach (float val in sorted)
        {
            if (gameController.life <= val)
            {
                dialoguePath = JsonManager.loadedData.gameEndConditions[val];
                break;
            }
        }
        UpdateDialogue();
        HideAllOptions();
        gameIsOver = true;
    }

    private List<float> GetSortedList(List<float> toSort)
    {
        List<float> sorted = new List<float>();
        while (sorted.Count < toSort.Count)
        {
            float smallest = 999;
            foreach (float val in toSort)
            {
                if (sorted.Contains(val) == false && val < smallest)
                {
                    smallest = val;
                }
            }
            sorted.Add(smallest);
        }
        return sorted;
    }
}

public class NewDialogue
{
    public List<string> _pathTillHere;
    public List<string> _allText;
    public List<string> _options;
    public List<float> _optionsWeight;
    public float _typeSpeed;
    public string _textColor;
    public Dictionary<string, bool> _imageConfig;


    public NewDialogue(List<string> pathTillHere, List<string> allText, List<string> options, List<float> optionsWeight, float typeSpeed, string textColor, Dictionary<string, bool> imageConfig)
    {
        _pathTillHere = pathTillHere;
        _allText = allText;
        _typeSpeed = typeSpeed;
        _textColor = textColor;
        _options = options;
        _imageConfig = imageConfig;
        _optionsWeight = optionsWeight;
    }
}

public class NewImage
{
    public string _imageName;
    public float _xPos;
    public float _yPos;
    public int _layer;

    public NewImage(string name, float xPos, float yPos, int layer)
    {
        _xPos = xPos;
        _yPos = yPos;
        _layer = layer;
        _imageName = name;
    }

    private Sprite mySprite;
    public void SetSprite(Sprite spr)
    {
        mySprite = spr;
    }
    public Sprite GetSprite()
    {
        return mySprite;
    }
}