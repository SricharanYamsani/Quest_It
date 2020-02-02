using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI frameCounterText;

    // Update is called once per frame
    void Update()
    {
        frameCounterText.text = ((int)(1f / Time.unscaledDeltaTime)).ToString();
    }
}
