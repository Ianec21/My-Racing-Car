using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    public string actualText = "";
    public Text textComponent;

    void Update()
    {
        textComponent.text = actualText;
    }
}
