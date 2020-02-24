using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI frameCounterText;

    private float waitTimer = 0.0f;

    private const float timer = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer + waitTimer)
        {
            frameCounterText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();

            waitTimer = Time.time;
        }
    }
}
