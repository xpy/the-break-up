using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueOptionPanel : MonoBehaviour
{

    public DialogueOption dialogueOption;
    public DialogueManager box;
    public List<DialogueOption> nextOptions;
    private List<Image> nextOptionImages;

    public void Start() {
        nextOptionImages = new List<Image>();
        List<Image> bla = new List(GetComponentsInChildren<Image>());
        foreach (Image nextOptionImage in bla)
        {
            if (nextOptionImage.name.StartsWith("topicImage", System.StringComparison.Ordinal)) {
                nextOptionImages.Add(nextOptionImage);
            }
        }
    }

    public void Update() { }

    public void SetNextOptions(List<DialogueOption> NextOptions)
    {
        nextOptions = NextOptions;
        for(int i = 0; i < nextOptions.Count; i++)
        {
            Debug.Log("Topic: " + nextOptions[i]);
            Sprite topic = Resources.Load<Sprite>("Sprites/topic-" + nextOptions[i].topic.Replace(" ", ""));
            Debug.Log("TopicSprite: "+ topic);

            print(nextOptionImages[i].name);
            nextOptionImages[i].sprite = topic;
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