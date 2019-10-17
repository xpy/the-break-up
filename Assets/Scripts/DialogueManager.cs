using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class DialogueManager: MonoBehaviour
{
    DialogueParser dialogueParser;
    Character character;
    
    public Dictionary<string, List<DialogueAnswer>> selectedAnswers = new Dictionary<string, List<DialogueAnswer>>();
    
    List<Button> buttons = new List<Button>();
    public Text dialogueBox;
    public Text characterNameBox;

    public bool answerred;
    public bool initialized;

    void Start()
    {
        dialogueBox = GameObject.Find("Answer").GetComponentInChildren<Text>();
        characterNameBox = GameObject.Find("CharacterName").GetComponentInChildren<Text>();

        buttons = new List<Button>(GameObject.Find("DialogueOptionPanel").GetComponentsInChildren<Button>());

        answerred = false;

        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        character = GameObject.Find("Character").GetComponent<Character>();
    }

    void PrepareAnswers() { 
        foreach(KeyValuePair<string, Dictionary<string, int>> topic in character.characterTraits){
            DialogueTopic dt;
            dialogueParser.dialogue.TryGetValue(topic.Key, out dt);

            if(dt == null)
            {
                Debug.Log("this should not happen, topic: " + topic.Key);
                continue;
            }
            foreach (KeyValuePair<string, int> subtopic in topic.Value)
            {
                DialogueSubTopic dst;

                dt.subTopics.TryGetValue(subtopic.Key, out dst);
                if (dst == null)
                {
                    Debug.Log("this should not happen, subtopic: " + subtopic.Key);
                    continue;
                }

                DialogueAnswer answer = dst.GetAnswer(subtopic.Value);
                if( answer == null)
                {
                    Debug.Log("this should not happen, modifier: " + subtopic.Value);
                    continue;
                }

                if(!selectedAnswers.ContainsKey(dt.name))
                {
                    selectedAnswers[dt.name] = new List<DialogueAnswer>();
                }
                selectedAnswers[dt.name].Add(answer);
            }
        }
    }

    void Update()
    {
        dialogueBox.text = "Answer be mine";

        if (!initialized)
        {
            PrepareAnswers();
            setDialogueAnswers(getRandomAnswers());
            initialized = true;
        }

        if (Input.GetMouseButtonDown(0) && answerred)
        {
            Debug.Log("here");
            UpdateDialogue();            
        }
        else
        {
        }
        UpdateUI();
    }

    private List<DialogueAnswer> getRandomAnswers()
    {
        List<string> keyList = new List<string>(selectedAnswers.Keys);
        System.Random rand = new System.Random();
        List<DialogueAnswer> result = new List<DialogueAnswer>();

        for (int i = 0; i < 3; i++)
        {
            string randomKey = keyList[rand.Next(keyList.Count)];
            keyList.Remove(randomKey);
            List<DialogueAnswer> topicAnswers = selectedAnswers[randomKey];
            result.Add(topicAnswers[rand.Next(topicAnswers.Count)]);
        }
        return result;
    }

    private void deleteAnswer(DialogueAnswer answer) {
        if (!selectedAnswers.ContainsKey(answer.topic))
        {
            Debug.Log("Couldn't find topic " + answer.topic);
            return;
        }
        selectedAnswers[answer.topic].Remove(answer);
        if(selectedAnswers[answer.topic].Count == 0)
        {
            selectedAnswers.Remove(answer.topic);
        }
    }

    private void setDialogueAnswers(List<DialogueAnswer> answers)
    {
        foreach (DialogueAnswer answer in answers) {
            
        }
    }

    private void setAnswer(string text)
    {
        dialogueBox.text = text;
    }

    public void UpdateUI()
    {
        
    }

    public void UpdateDialogue()
    {
        setAnswer("");
    }

    //void CreateButtons()
    //{
    //    for (int i = 0; i < options.Length; i++)
    //    {
    //        GameObject button = (GameObject)Instantiate(choiceBox);
    //        Button b = button.GetComponent<Button>();
    //        ChoiceButton cb = button.GetComponent<ChoiceButton>();
    //        cb.SetText(options[i].Split(':')[0]);
    //        cb.option = options[i].Split(':')[1];
    //        cb.box = this;
    //        b.transform.SetParent(this.transform);
    //        b.transform.localPosition = new Vector3(0, -25 + (i * 50));
    //        b.transform.localScale = new Vector3(1, 1, 1);
    //        buttons.Add(b);
    //    }
    //}

    void ClearButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            print("Clearing buttons");
            Button b = buttons[i];
            buttons.Remove(b);
            Destroy(b.gameObject);
        }
    }
}
