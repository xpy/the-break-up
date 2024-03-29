﻿using System.Collections;
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
    CharacterParser characterParser;
    
    public Dictionary<string, List<DialogueOption>> selectedOptions = new Dictionary<string, List<DialogueOption>>();
    public Dictionary<string, TopicIndicator> topicIndicators= new Dictionary<string, TopicIndicator>();

    DialogueOptionPanel[] panels;
    public Text answerBox;
    public Text characterNameBox;
    
    public DialogueOptionPanel choiceOptionPicked;
    private bool initialized = false;
    public bool deletionEnabled;

    public int round = 0;
    System.Random rand = new System.Random();

    public Dictionary<string, TopicScore> topicScores = new Dictionary<string, TopicScore>();
    public bool mouseButtonWasDown = false;
    void Start()
    {
        dialogueParser = GameObject.Find("DialogueParser").GetComponent<DialogueParser>();
        characterParser = GameObject.Find("CharacterParser").GetComponent<CharacterParser>();
        
        answerBox = GameObject.Find("Answer").GetComponent<Text>();
        panels = GetComponentsInChildren<DialogueOptionPanel>();
        characterNameBox = GameObject.Find("CharacterName").GetComponent<Text>();
        characterNameBox.text = characterParser.characterName;
    }

    void PrepareAnswers() {
        foreach(KeyValuePair<string, Dictionary<string, int>> topic in characterParser.characterTraits){
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
            TopicIndicator[] indicators = GameObject.Find("TopicIndicators").GetComponentsInChildren<TopicIndicator>();
            foreach (TopicIndicator topicIndicator in indicators)
            {
                topicIndicators.Add(topicIndicator.topic, topicIndicator);
            }

            foreach (string topic in dialogueParser.dialogue.Keys)
            {
                topicScores[topic] = new TopicScore(0, 0);
                topicIndicators[topic].topicScore = topicScores[topic];
            }

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
            mouseButtonWasDown = false;
        }
    }

    private List<DialogueOption>  getRandomQuestions()
    {
        List<string> keyList = new List<string>(selectedOptions.Keys);
        
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
            return;
        }
        selectedOptions[dialogueOption.topic].Remove(dialogueOption);
    
        if(selectedOptions[dialogueOption.topic].Count == 0)
        {
            selectedOptions.Remove(dialogueOption.topic);
        }
    }

    private void setDialogueOptions(List<DialogueOption> options)
    {
        if (deletionEnabled)
        {
            foreach (DialogueOption option in options)
            {
                deleteOption(option);
            }
        }
        round++;
        for (int i = 0; i < options.Count; i++)
        {
            DialogueOption option = options[i];
            DialogueOptionPanel panel = panels[i];
            panel.Reset();

            DialogueOptionPanel dop = panel.GetComponent<DialogueOptionPanel>();
            dop.SetDialogueOption(option);
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
            TopicScore score = topicScores[dialogueOption.topic];
            score.total++;
            score.currentScore += dialogueOption.modifier;
            printScore();
            choiceOptionPicked = optionPanel;
        }
    }

    private void printScore()
    {
        TopicScore totals = new TopicScore(0, 0);
        string result = "";

        foreach (KeyValuePair<string, TopicScore> score in topicScores)
        {
            result += "\n" + score.Key + ": total=" + score.Value.total + " score=" + score.Value.currentScore;
            totals.total += score.Value.total;
            totals.currentScore += score.Value.currentScore;
        }

        float x = 1 - (float)totals.currentScore / totals.total;
        GameObject theWoman = GameObject.Find("the-woman");
        Debug.Log("AND THE SCOPRE IS!!! ");
        print(""+x);

        theWoman.GetComponent<TheWoman>().status = x;

        result = "Totals: total=" + totals.total + " score=" + totals.currentScore + result;
        print(result);
    }

    public void UpdateDialogue()
    {
        setDialogueOptions(choiceOptionPicked.nextOptions);
        choiceOptionPicked = null;
        answerBox.text = "";
    }
}
