using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text infoText;

    public void SetInfoText(string stringText)
    {
        infoText.text = stringText;
        infoText.SetAllDirty();
    }
}
