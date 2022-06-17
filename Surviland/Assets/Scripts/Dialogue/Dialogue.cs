using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{

    [SerializeField] private Text textObject;
    // [SerializeField] private Color keyColor;
    [SerializeField] private KeyCode keyNeeded = KeyCode.Return;
    [SerializeField] private string keyNeededDisplay = "Space";
    [SerializeField] private float typeDelay;
    [SerializeField] private List<string> allDialogue;

    [SerializeField][Range(0, 1)] private float keyAlphaWhenTyping = 0;
    [SerializeField][Range(0, 1)] private float keyAlphaWhenCompleted = 0;

    private Text keyTextObj;

    private string displayText;

    private int dialogueToDisplay;

    private float timer = 0;

    private bool currentDialogueCompleted => allDialogue.Count <= dialogueToDisplay || displayText == allDialogue[dialogueToDisplay];

    private bool isThereMoreDialogue => dialogueToDisplay < allDialogue.Count - 1;

    private DialogueController diagController;

    private bool waitingPlayerChoice;


    // Start is called before the first frame update
    void Start()
    {
        waitingPlayerChoice = false;

        displayText = "";
        dialogueToDisplay = 0;

        keyTextObj = transform.Find("Key").GetComponent<Text>();

        SetKeyTextAlpha(keyAlphaWhenTyping);

        diagController = transform.GetComponentInParent<DialogueController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allDialogue == null) return;

        if ((Input.GetKeyDown(keyNeeded) || Input.GetMouseButtonDown(0)) && !waitingPlayerChoice)
        {
            // player control over dialogue
            if (currentDialogueCompleted)
            {
                dialogueToDisplay++;
                ClearDisplayText();
            }
            else
            {
                displayText = allDialogue[dialogueToDisplay];
                UpdateDisplayText();
            }
        }


        if (currentDialogueCompleted)
        {
            if (isThereMoreDialogue)
            {
                SetKeyTextAlpha(keyAlphaWhenCompleted);
            }
            else
            {
                SetKeyTextAlpha(0);
                if (!waitingPlayerChoice)
                {
                    diagController.ShowOptionsForCurrentDialogue();
                    waitingPlayerChoice = true;
                }
            }
            return;
        }

        SetKeyTextAlpha(keyAlphaWhenTyping);


        timer += Time.deltaTime;

        if (timer >= typeDelay)
        {
            AddLetter();
            UpdateDisplayText();
        }
    }

    private void InitializeDialogue(List<string> _allDialogue)
    {
        allDialogue = _allDialogue;
        dialogueToDisplay = 0;
    }

    // public void InitializeDialogue(List<string> _allDialogue, float _typeSpeed)
    // {
    //     InitializeDialogue(_allDialogue);
    //     typeDelay = _typeSpeed;
    // }



    public void InitializeNewDialogue(NewDialogue dialogue)
    {
        typeDelay = dialogue._typeSpeed;
        InitializeDialogue(dialogue._allText);
    }


    private void AddLetter()
    {
        int nextLetterIndex = displayText.Length;
        displayText += allDialogue[dialogueToDisplay].ToCharArray()[nextLetterIndex];
        timer = 0;
    }

    private void UpdateDisplayText()
    {
        textObject.text = displayText;
    }

    private void ClearDisplayText()
    {
        displayText = "";
        UpdateDisplayText();
    }

    private void SetKeyTextAlpha(float alpha)
    {
        if (alpha != keyTextObj.color.a)
        {
            Color col = keyTextObj.color;
            col.a = alpha;
            keyTextObj.color = col;
        }
    }

    public void ClearAllDialogueBox()
    {
        waitingPlayerChoice = false;
        ClearDisplayText();
        keyTextObj.text = "";
        allDialogue = null;
    }

}
