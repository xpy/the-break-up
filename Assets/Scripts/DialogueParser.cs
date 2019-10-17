using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;

public class DialogueOption
{
    public string sentence, answer, topic, subtopic;
    public int modifier;


    public DialogueOption(string Topic, string Subtopic, string Sentence, string Answer, string Modifier)
    {
        topic = Topic;
        subtopic = Subtopic;
        sentence = Sentence;
        answer = Answer;
        modifier = Int32.Parse(Modifier);
    }

    public override string ToString()
    {
        return "DialogueAnswer: topic = "+ topic + "subtopic = " + subtopic+ " sentence = *" + sentence + "* answer = *" + answer + "* modifier = *" + modifier + "*";
    }
}

public class DialogueTopic
{
    public string name;
    public Dictionary<string, DialogueSubTopic> subTopics = new Dictionary<string, DialogueSubTopic>();

    public DialogueTopic(string Name)
    {
        name = Name;
    }

    public DialogueSubTopic GetSubTopic(string name)
    {
        if (subTopics.ContainsKey(name))
        {
            return subTopics[name];
        }
        DialogueSubTopic newSubTopic = new DialogueSubTopic(name);
        subTopics.Add(name, newSubTopic);
        return newSubTopic;

    }
}

public class DialogueSubTopic
{
    public string name;
    private List<DialogueOption> answersPositive = new List<DialogueOption>();
    private List<DialogueOption> answersNegative = new List<DialogueOption>();

    public DialogueSubTopic(string Name)
    {
        name = Name;
    }

    public DialogueOption GetAnswer(int modifier)
    {
        List<DialogueOption> selectedAnswers = answersPositive;
        if (modifier == 0)
        {
            selectedAnswers = answersNegative;
        }
        if (selectedAnswers.Count == 0)
        {
            return null;
        }
        return selectedAnswers[0]; //use the first option for now
    }

    public void AddAnswer(DialogueOption answer)
    {
        if (answer.modifier == 0)
        {
            answersNegative.Add(answer);
        }
        else
        {
            answersPositive.Add(answer);
        }
    }
}

public class DialogueParser : MonoBehaviour
{
    public Dictionary<string, DialogueTopic> dialogue;
    public List<string> topics;
    public List<string> subtopics;

    // Start is called before the first frame update
    void Start()
    {
        string file = "Assets/Data/Dialogue";
        string sceneNum = EditorApplication.currentScene;
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".tsv";

        dialogue = new Dictionary<string, DialogueTopic>();

        LoadDialogue(file);
    }

    DialogueTopic GetTopic(string name)
    {
        if (dialogue.ContainsKey(name))
        {
            return dialogue[name];
        }
        DialogueTopic newTopic = new DialogueTopic(name);
        dialogue.Add(name, newTopic);
        return newTopic;
    }

    void LoadDialogue(string filename)
    {
        string line;
        StreamReader r = new StreamReader(filename);

        using (r)
        {
            do
            {
                line = r.ReadLine();
                if (line != null)
                {
                    string[] lineData = line.Split('\t');
                    if (lineData[0] == "topic" || lineData[0] == "")
                    {
                        continue; // this is the index line
                    }                    

                    string topic = lineData[0];
                    string subtopic = lineData[1];
                    string sentence = lineData[2];
                    string answer = lineData[3];
                    string modifier = lineData[4];

                    GetTopic(topic).GetSubTopic(subtopic).AddAnswer(
                        new DialogueOption(
                            topic, subtopic, sentence, answer, modifier)
                       );
                    topics.Add(topic);
                    subtopics.Add(subtopic);

                    //Debug.Log(GetTopic(topic).GetSubTopic(subtopic).GetAnswer(modifier));
                }
            }
            while (line != null);
            r.Close();
        }
    }

    public List<string> GetTopics()
    {
        return new List<string>(topics);
    }

    public List<string> GetSubTopics()
    {
        return new List<string>(subtopics);
    }

    // Update is called once per frame
    void Update() { }
}
