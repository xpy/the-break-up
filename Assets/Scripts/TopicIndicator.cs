using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopicIndicator : MonoBehaviour
{
    public float x;
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
        GetComponent<Image>().color = new Color(2.0f * x, 2.0f * (1 - x), 0);
    }

    public void SetTopicScore(DialogueManager.TopicScore score)
    {
        x =(float) score.currentScore / score.total;
    }
}
