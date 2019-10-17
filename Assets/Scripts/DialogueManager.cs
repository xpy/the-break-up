using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager: MonoBehaviour
{
    DialogueParser dialogueParser;
    CharacterParser characterParser;

    public string characterName="Susi";

    public string answer;

    List<DialogueAnswer> selectedAnswers;
    List<Button> buttons = new List<Button>();
    public Text dialogueBox;
    public Text nameBox;
    public GameObject choiceBox;

    // Start is called before the first frame update
    void Start()
    {
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        characterParser = GameObject.Find("CharacterParser").GetComponent<CharacterParser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("here");
            ShowDialogue();
        }
    }

    public void ShowDialogue()
    {
        //ResetImages();
    }
}
