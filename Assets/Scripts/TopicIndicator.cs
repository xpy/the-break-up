using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicIndicator : MonoBehaviour
{
    public DialogueManager.TopicScore topicScore;
    public string topic;
    // Start is called before the first frame update
    void Start()
    {
        Image[] images = this.GetComponentsInChildren<Image>();
        Image topicImage = null;
        foreach(Image image in images)
        {
            if(image.name == "Topic")
            {
                topicImage = image;
            }
        }
        topicImage.sprite = Resources.Load<Sprite>("Sprites/topic-" + topic.Replace(" ", ""));
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        if (topicScore == null)
        {
            return;
        }
        float x = 1.0f - ((float)topicScore.currentScore / topicScore.total);
        GetComponent<Image>().color = new Color(2.0f * x, 2.0f * (1 - x), 0);
    }
}
