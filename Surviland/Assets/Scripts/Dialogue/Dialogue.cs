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

    private bool currentDialogueCompleted => displayText == allDialogue[dialogueToDisplay];

    private bool isThereMoreDialogue => dialogueToDisplay < allDialogue.Count;

    private DialogueController diagController;


    // Start is called before the first frame update
    void Start()
    {
        displayText = "";
        dialogueToDisplay = 0;

        keyTextObj = transform.Find("Key").GetComponent<Text>();
        // keyTextObj.color = keyColor;
        keyTextObj.text = "> " + keyNeeded;
        SetKeyTextAlpha(keyAlphaWhenTyping);

        diagController = transform.GetComponentInParent<DialogueController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (allDialogue == null) return;


        if (Input.GetKeyDown(keyNeeded))
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

        if (isThereMoreDialogue == false)
        {
            // dialogue is all done, delete obj
            if (diagController.ShowOptionsForCurrentDialogue() == false)
            {
                ClearAllDialogueBox();
            }
            return;
        }


        if (currentDialogueCompleted)
        {
            SetKeyTextAlpha(keyAlphaWhenCompleted);
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

    public void InitializeDialogue(List<string> _allDialogue)
    {
        allDialogue = _allDialogue;
    }

    public void InitializeDialogue(List<string> _allDialogue, float _typeSpeed)
    {
        InitializeDialogue(_allDialogue);
        typeDelay = _typeSpeed;
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

    private void ClearAllDialogueBox()
    {
        ClearDisplayText();
        keyTextObj.text = "";
    }

}
