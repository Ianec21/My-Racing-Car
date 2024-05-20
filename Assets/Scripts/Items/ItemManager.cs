using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [Header("Item Data")]
    public string itemName;
    public bool canBeGrabbed;
    public bool isGrabbed;
    public bool isUsable;
    public bool isAttachment = false;
}