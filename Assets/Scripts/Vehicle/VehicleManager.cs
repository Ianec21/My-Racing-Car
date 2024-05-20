using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public AttachmentManager[] attachments;

    public bool areAttachmentsInstalled()
    {
        bool good = true;
        foreach(var attachment in attachments)
        {
            if (attachment.isDetached())
            {
                Debug.Log(attachment.name + " is not attached!");
                good = false;
                break;
            }
        }

        return good;
    }
}
