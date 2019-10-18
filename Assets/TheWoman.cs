using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWoman : MonoBehaviour
{
    Animator anim;
    public float status;
    private int realStatus;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        realStatus = Mathf.FloorToInt(status * 4f);
        anim.SetInteger("angryness", realStatus);
    }
}
