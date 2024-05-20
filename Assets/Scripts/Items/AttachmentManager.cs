using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentManager : MonoBehaviour
{
    public enum AttachmentType
    {
        AIRFILTER,
        CAR_BATTERY
    }
    
    public enum AttachmentState
    {
        ATTACHED,
        DETACHED,
        WORLD
    }

    public AttachmentType attachmentType;
    public AttachmentState attachmentState;
    public AudioClip installSound;

    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        switch(attachmentState)
        {
            case AttachmentState.DETACHED:

                Transform parent = transform.parent;
                
                if(parent != null)
                {
                    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
                    meshRenderer.enabled = false;

                    Rigidbody rb = GetComponent<Rigidbody>();
                    rb.isKinematic = true;
                }

                break;
        }

        originalColor = GetComponent<MeshRenderer>().material.color;
    }

    public void HandleAttachItem()
    {
        switch(attachmentState)
        {
           case AttachmentState.ATTACHED: DetachItem(); break;
           case AttachmentState.DETACHED: AttachItem(); break;
        }
    }

    public void DetachItem()
    {
        attachmentState = AttachmentState.DETACHED;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        AudioManager.instance.PlaySoundFXClip(installSound);
    }

    public void AttachItem()
    {
        attachmentState = AttachmentState.ATTACHED;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        AudioManager.instance.PlaySoundFXClip(installSound);
    }

    public void ShowTransparent(GameObject gameObject, bool state)
    {
        if (state)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;

            Color color = meshRenderer.material.color;
            color.r = 0f;
            color.g = 1f;
            color.b = 0f;
            color.a = 0.5f;
            meshRenderer.material.color = color;
        } else
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.enabled = false;

            meshRenderer.material.color = originalColor;
        }
    }

    public void RestoreState()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        Color color = meshRenderer.material.color;
        color.r = 1f;
        color.g = 1f;
        color.b = 1f;
        color.a = 1f;
        meshRenderer.material.color = color;
        attachmentState = AttachmentState.WORLD;
    }

    public bool isAttached()
    {
        return attachmentState == AttachmentState.ATTACHED;
    }

    public bool isDetached()
    {
        return attachmentState == AttachmentState.DETACHED;
    }

    public bool isWorld()
    {
        return attachmentState == AttachmentState.WORLD;
    }
}
