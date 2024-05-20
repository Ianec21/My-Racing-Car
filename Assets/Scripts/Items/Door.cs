using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorTypeEnum
    {
        FRONT_LEFT,
        FRONT_RIGHT,
        REAR_LEFT,
        REAR_RIGHT,
        TRUNK,
        HOOD,
        HOUSE_DOOR,
        GARAGE_DOOR
    }

    public enum DoorStateEnum
    {
        OPENED,
        CLOSED
    }

    public DoorTypeEnum type;
    public DoorStateEnum state;

    public Vector3 openedRotation;
    public Vector3 openedPosition;
    private Quaternion initialRotation;
    private Vector3 initialPosition;

    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private void Start()
    {
        initialRotation = transform.localRotation;
        initialPosition = transform.localPosition;
    }

    public void HandleDoor()
    {
        switch(state)
        {
            case DoorStateEnum.OPENED: CloseDoor(); break;
            case DoorStateEnum.CLOSED: OpenDoor(); break;
        }
    }

    private void OpenDoor()
    {
        transform.localRotation = Quaternion.Euler(openedRotation.x, openedRotation.y, openedRotation.z);
        transform.localPosition = openedPosition;
        state = DoorStateEnum.OPENED;

        AudioManager.instance.PlaySoundFXClip(openDoorSound);
    }

    private void CloseDoor()
    {
        transform.localRotation = initialRotation;
        transform.localPosition = initialPosition;
        state = DoorStateEnum.CLOSED;
        AudioManager.instance.PlaySoundFXClip(closeDoorSound);
    }
}
