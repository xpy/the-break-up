using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChoiceButton : MonoBehaviour
{

    public DialogueOption dialogueOption;
    public DialogueManager box;
    public List<DialogueOption> nextOptions;

    public void Start() { }

    public void Update() { }

    public void SetText(string newText)
    {
        this.GetComponentInChildren<Text>().text = newText;
    }

    public void OnClick()
    {
        box.OptionPicked(this);
    }
}