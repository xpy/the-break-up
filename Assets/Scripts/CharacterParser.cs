using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class CharacterParser : MonoBehaviour
{
    Dictionary<string, Dictionary<string, string>> characterTraits;

    // Start is called before the first frame update
    void Start()
    {
        string file = "Assets/Data/Character";
        string sceneNum = EditorApplication.currentScene;
        sceneNum = Regex.Replace(sceneNum, "[^0-9]", "");
        file += sceneNum;
        file += ".tsv";

        characterTraits = new Dictionary<string, Dictionary<string, string>>();

        LoadCharacterTraits(file);
    }

    void LoadCharacterTraits(string filename)
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
                    if (lineData[0] == "topic")
                    {
                        continue; // this is the index line
                    }
                    string topic = lineData[0];
                    string subtopic = lineData[1];
                    string modifier = lineData[2];

                    if (!characterTraits.ContainsKey(topic))
                    {
                        characterTraits.Add(topic, new Dictionary<string, string>());
                    }

                    characterTraits[topic][subtopic] = modifier;

                    Debug.Log(topic + ", " + subtopic + ", " + characterTraits[topic][subtopic]);
                }
            }
            while (line != null);
            r.Close();
        }
    }

    // Update is called once per frame
    void Update() { }
}