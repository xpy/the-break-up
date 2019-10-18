using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MadnessIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMadness(DialogueManager.TopicScore score)
    {
        print((float)score.currentScore / score.total);
        int x = (int)(510 * ((float)score.currentScore/score.total));
        print(x);
        int r = 0;
        int g = 0;
        int b = 0;
        if (x > 510)
        {
            r = 
            g = 255;
        }
        if (x > 255){
            r = x - 255;
            g = 0;
        }
        else
        {
            r = 255;
            g = x;
        }

        print(r + " " + g + " " + b);
        GetComponent<Image>().color = new Color(r, g, b, 255);
    }
}
