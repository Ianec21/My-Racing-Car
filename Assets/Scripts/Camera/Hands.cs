using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    private ItemManager itemManager;
    private AttachmentManager attachmentManager;
    private IgnitionScript ignitionScript;
    private Door doorHandler;
    private VehicleManager vehicleManager;
    public float handsDistance = 2.0f;
    public InfoText infoText;
    public float dropForce = 5.0f;
    private float ZRotation = 0f;
    public float rotationOffset = 2f;
    public LayerMask objectLayer;

    private Collider hitCollider = null;
    private GameObject holdingObject = null;
    RaycastHit hit;

    void Start()
    {
        vehicleManager = GameObject.Find("VAZ").GetComponent<VehicleManager>();
    }

    void Update()
    {
        HandleObjectInteract();
        HandleObjectRotate();

        if (hitCollider != null)
        {
            itemManager = hitCollider.gameObject.GetComponent<ItemManager>();
            doorHandler = hitCollider.gameObject.GetComponent<Door>();
            attachmentManager = hitCollider.gameObject.GetComponent<AttachmentManager>();
            ignitionScript = hitCollider.gameObject.GetComponent<IgnitionScript>();
            
            if (itemManager)
            {
                if (holdingObject == null || (attachmentManager != null && (attachmentManager.isAttached() || attachmentManager.isWorld())))
                {
                    infoText.actualText = itemManager.itemName.Length > 0 ? itemManager.itemName : "No Named Item";
                }
            }
        }
        else infoText.actualText = "";
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, handsDistance, objectLayer.value))
        {
            hitCollider = hit.collider;
        }
        else hitCollider = null;

        Debug.DrawRay(transform.position, transform.forward * handsDistance, Color.yellow);

        HandleObjectThrow();
    }

    //custom methods
    void HandleObjectInteract()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (hitCollider != null && itemManager && doorHandler)
            {
                doorHandler.HandleDoor();

                return;
            }

            if (ignitionScript != null)
            {
                Debug.Log("GO!!");
                if (ignitionScript.ignitionState == false)
                {
                    bool installedAttachments = vehicleManager.areAttachmentsInstalled();
                    Debug.Log(installedAttachments);

                    if (installedAttachments)
                    {
                        ignitionScript.ignitionState = true;
                        ignitionScript.PlayAudioStartEngine();
                    }
                }
                else ignitionScript.ignitionState = false;

                return;
            }

            if (holdingObject == null && (attachmentManager != null && attachmentManager.isAttached() | attachmentManager.isWorld()))
            {

                if (hitCollider != null && holdingObject == null && itemManager && itemManager.canBeGrabbed && hitCollider.transform.parent == null)
                {
                    hitCollider.transform.parent = transform;
                    hitCollider.GetComponent<Collider>().enabled = false;
                    hitCollider.GetComponent<Rigidbody>().useGravity = false;
                    hitCollider.GetComponent<Rigidbody>().isKinematic = true;
                    hitCollider.GetComponent<Rigidbody>().freezeRotation = true;
                    holdingObject = hitCollider.gameObject;

                    if (attachmentManager != null)
                    {
                        GameObject similarObject = GameObject.Find("/VAZ/VazObject/" + hitCollider.gameObject.name.Replace("(Clone)", ""));
                        attachmentManager.ShowTransparent(similarObject, true);
                    }

                    return;
                }

                if(hitCollider != null && itemManager && attachmentManager && hitCollider.transform.parent != null)
                {
                    holdingObject = GameObject.Instantiate(hitCollider).gameObject;
                    AttachmentManager holdingObjectAttachmentManager = holdingObject.GetComponent<AttachmentManager>();
                    holdingObjectAttachmentManager.RestoreState();

                    holdingObject.transform.position = hitCollider.transform.position;
                    holdingObject.transform.localRotation = hitCollider.transform.localRotation;
                    holdingObject.transform.parent = transform;

                    holdingObject.GetComponent<Collider>().enabled = false;
                    holdingObject.GetComponent<Rigidbody>().useGravity = false;
                    holdingObject.GetComponent<Rigidbody>().isKinematic = true;
                    holdingObject.GetComponent<Rigidbody>().freezeRotation = true;

                    attachmentManager.HandleAttachItem();

                    GameObject similarObject = GameObject.Find("/VAZ/VazObject/" + hitCollider.gameObject.name);
                    attachmentManager.ShowTransparent(similarObject, true);

                    holdingObjectAttachmentManager.RestoreState();

                    return;
                }
            }
            else
            {
                if (holdingObject)
                {
                    AttachmentManager holdingObjectAttachmentManager = holdingObject.GetComponent<AttachmentManager>();
                    if (holdingObjectAttachmentManager != null)
                    {
                        if (hitCollider != null && itemManager && attachmentManager && hitCollider.name == holdingObject.name.Replace("(Clone)", ""))
                        {
                            GameObject similarObject = GameObject.Find("/VAZ/VazObject/" + hitCollider.gameObject.name);
                            attachmentManager.ShowTransparent(similarObject, false);
                            attachmentManager.AttachItem();
                            Destroy(holdingObject);

                            holdingObject = null;
                        }
                        else
                        {
                            DropObject();
                        }

                        return;
                    }

                    DropObject();
                }
            }
        }
    }

    void HandleObjectThrow()
    {
        if (holdingObject)
        {
            if (Input.GetMouseButton(1))
            {
                if (holdingObject != null)
                {
                    //disable isKinematic first in order to apply force
                    holdingObject.GetComponent<Rigidbody>().isKinematic = false;
                    holdingObject.GetComponent<Rigidbody>().AddForce(transform.forward * dropForce * 100f * Time.fixedDeltaTime, ForceMode.Impulse);
                    DropObject();
                }
            }
        }
    }

    void DropObject()
    {
        if (attachmentManager != null)
        {
            //replace (Clone) from object's name when instantiate it
            GameObject similarObject = GameObject.Find("/VAZ/VazObject/" + holdingObject.gameObject.name.Replace("(Clone)", ""));
            Debug.Log(holdingObject.gameObject.name.Replace("(Clone)", ""));
            attachmentManager.ShowTransparent(similarObject, false);
        }

        holdingObject.transform.parent = null;
        holdingObject.GetComponent<Collider>().enabled = true;
        holdingObject.GetComponent<Rigidbody>().useGravity = true;
        holdingObject.GetComponent<Rigidbody>().isKinematic = false;
        holdingObject.GetComponent<Rigidbody>().freezeRotation = false;
        holdingObject = null;
    }

    //needs some work
    void HandleObjectRotate()
    {
        if (holdingObject != null)
        {
            if(Input.GetAxisRaw("Mouse ScrollWheel") > 0f) //forward
            {
                ZRotation += rotationOffset;
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) //backwards
            {
                ZRotation -= rotationOffset;
            }

            holdingObject.transform.rotation = Quaternion.Euler(holdingObject.transform.rotation.x, ZRotation, -ZRotation);
        }
    }
}
