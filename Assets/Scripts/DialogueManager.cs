using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class DialogueManager: MonoBehaviour
{
    public class TopicScore
    {
        public int currentScore;
        public int total;

        public TopicScore(int CurrentScore, int Total)
        {
            currentScore = CurrentScore;
            total = Total;
        }
    }

    DialogueParser dialogueParser;
    Character character;
    
    public Dictionary<string, List<DialogueOption>> selectedOptions = new Dictionary<string, List<DialogueOption>>();

    DialogueOptionPanel[] panels;
    public Text answerBox;
    public Text characterNameBox;

    public DialogueOptionPanel choiceOptionPicked;
    public bool initialized;

    public GameObject choiceBox;
    public int round = 0;
    
    public Dictionary<string, TopicScore> topicScores = new Dictionary<string, TopicScore>();
    public bool mouseButtonWasDown = false;
    void Start()
    {
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        character = GameObject.Find("Character").GetComponent<Character>();

        answerBox = GameObject.Find("Answer").GetComponent<Text>();
        panels = GetComponentsInChildren<DialogueOptionPanel>();
        characterNameBox = GameObject.Find("CharacterName").GetComponent<Text>();
        characterNameBox.text = character.characterName;
        foreach(string topic in dialogueParser.dialogue.Keys)
        {
            topicScores[topic] = new TopicScore(0, 0);
        }
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

                DialogueOption answer = dst.GetAnswer(subtopic.Value);
                if( answer == null)
                {
                    Debug.Log("this should not happen, modifier: " + subtopic.Value);
                    continue;
                }

                if(!selectedOptions.ContainsKey(dt.name))
                {
                    selectedOptions[dt.name] = new List<DialogueOption>();
                }
                selectedOptions[dt.name].Add(answer);
            }
        }
    }

    void Update()
    {
        if (!initialized)
        {
            answerBox.text = "";
            PrepareAnswers();
            initialized = true;
            setDialogueOptions(getRandomQuestions());
        }

        if (Input.GetMouseButtonDown(0) && choiceOptionPicked != null)
        {
            mouseButtonWasDown = true;
        }
        else if (Input.GetMouseButtonUp(0) && choiceOptionPicked != null && mouseButtonWasDown)
        {
            Debug.Log("I shall update");
            UpdateDialogue();
            Debug.Log("I have updated");
        }
    }

    private List<DialogueOption> getRandomQuestions()
    {
        List<string> keyList = new List<string>(selectedOptions.Keys);
        System.Random rand = new System.Random();
        List<DialogueOption> result = new List<DialogueOption>();

        for (int i = 0; i < 3; i++)
        {
            string randomKey = keyList[rand.Next(keyList.Count)];
            keyList.Remove(randomKey);
            List<DialogueOption> topicAnswers = selectedOptions[randomKey];
            result.Add(topicAnswers[rand.Next(topicAnswers.Count)]);
        }
        return result;
    }

    private void deleteOption(DialogueOption dialogueOption) {
        if (!selectedOptions.ContainsKey(dialogueOption.topic))
        {
            Debug.Log("Couldn't find topic " + dialogueOption.topic);
            return;
        }
        selectedOptions[dialogueOption.topic].Remove(dialogueOption);
    
        if(selectedOptions[dialogueOption.topic].Count == 0)
        {
            selectedOptions.Remove(dialogueOption.topic);
            Debug.Log("Removed topic: " + dialogueOption.topic);
        }
    }

    private void setDialogueOptions(List<DialogueOption> options)
    {
        foreach (DialogueOption option in options)
        {
            deleteOption(option);
        }
        round++;
        for (int i = 0; i < options.Count; i++)
        {
            DialogueOption option = options[i];
            DialogueOptionPanel panel = panels[i];

            DialogueOptionPanel dop = panel.GetComponent<DialogueOptionPanel>();
            dop.SetText(option.sentence);
            dop.dialogueOption = option;
            dop.SetNextOptions(getRandomQuestions());
            dop.box = this;
        }
    }

    public void OptionPicked(DialogueOptionPanel optionPanel)
    {
        if (choiceOptionPicked == null)
        {
            DialogueOption dialogueOption = optionPanel.dialogueOption;
            answerBox.text = dialogueOption.answer;
            Debug.Log(answerBox.text);
            TopicScore score = topicScores[dialogueOption.topic];
            score.total = score.total + 1;
            score.currentScore += dialogueOption.modifier;
            Debug.Log(dialogueOption.topic + " " + score.total + " " + score.currentScore);
            choiceOptionPicked = optionPanel;
        }
    }

    public void UpdateDialogue()
    {
        setDialogueOptions(choiceOptionPicked.nextOptions);
        choiceOptionPicked = null;
        answerBox.text = "";
    }
}
