using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{    
    float timercount;

    // Update is called once per frame
    void Update()
    {
        timercount += Time.deltaTime;
    }
}
