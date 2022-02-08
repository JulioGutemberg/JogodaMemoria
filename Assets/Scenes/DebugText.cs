using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugText : MonoBehaviour
{
    public TMP_Text debug;
    float count;
    void Update()
    {
        count += Time.deltaTime;
        debug.text = count + "";
    }
}
