using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChoiceButton : MonoBehaviour
{
    public void Start() { }

    public void Update() { }

    public void OnClick()
    {
        GetComponentInParent<DialogueOptionPanel>().OnClick();
    }
}