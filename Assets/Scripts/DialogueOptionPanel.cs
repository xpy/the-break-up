using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueOptionPanel : MonoBehaviour
{

    public DialogueOption dialogueOption;
    public DialogueManager box;
    public List<DialogueOption> nextOptions;
    private Image[] nextOptionImages;

    public void Start() {
        nextOptionImages = GetComponentsInChildren<Image>();
    }

    public void Update() {

    }

    public void SetNextOptions(List<DialogueOption> NextOptions)
    {
        nextOptions = NextOptions;
        for(int i = 0; i < nextOptions.Count; i++)
        {
            Sprite topic = Resources.Load<Sprite>("Sprites/topic-" + nextOptions[i].topic);

            //nextOptionImages[i].;
        }
    }

    public void SetText(string newText)
    {
        this.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = newText;
    }

    public void OnClick()
    {
        box.OptionPicked(this);
    }
}