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
    public Color selectedColor;
    public Color deselectedColor = Color.white;

    public void Start() {
        nextOptionImages = new List<Image>();

        List<Image> bla = new List<Image>(GetComponentsInChildren<Image>());
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
            Sprite topic = Resources.Load<Sprite>("Sprites/topic-" + nextOptions[i].topic.Replace(" ", ""));
            nextOptionImages[i].sprite = topic;
        }
    }
    public void SetDialogueOption(DialogueOption option)
    {
        SetButtonImage(option.topic);
        SetText(option.sentence);
        dialogueOption = option;
    }

    public void SetButtonImage(string topic)
    {
        Sprite topicSprite = Resources.Load<Sprite>("Sprites/topic-" + topic.Replace(" ", ""));
        Image[] images = this.GetComponentInChildren<Button>().GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            if(image.name == "Image")
            {
                image.sprite = topicSprite;
            }
        }
    }

    public void SetText(string newText)
    {
        this.GetComponentInChildren<Button>().GetComponentInChildren<Text>().text = newText;
    }

    private Image getButtonImage()
    {
        return this.GetComponentInChildren<Button>().GetComponent<Image>();
    }

    public void OnClick()
    {
        getButtonImage().color = selectedColor;
        box.OptionPicked(this);
    }

    public void Reset()
    {
        getButtonImage().color = deselectedColor;
    }
}