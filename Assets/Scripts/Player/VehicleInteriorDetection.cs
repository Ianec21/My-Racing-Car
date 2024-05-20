using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInteriorDetection : MonoBehaviour
{
    private CharacterController characterController;
    private GameObject player;
    private GameObject car;
    public PlayerController playerController;

    private bool hitCollider;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        player = GameObject.Find("Player");
        car = GameObject.Find("VAZ");
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return))
        {
            if(hitCollider && playerController.inCar)
            {
                if(car.GetComponent<CarController>().getSpeed() <= 1)
                {
                    playerController.inCar = false;
                    player.transform.parent = null;
                    player.GetComponent<CharacterController>().enabled = true;
                    player.GetComponent<PlayerController>().enabled = true;
                }
            } else if(hitCollider && !playerController.inCar)
            {
                playerController.inCar = true;
                player.transform.parent = car.transform;
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerController>().enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VehicleInteriorCollider")
        {
            characterController.height = 1.5f;
            //characterController.radius = 0.2f;
            hitCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VehicleInteriorCollider")
        {
            print("EXITED INTERIOR COLLIDER");
            characterController.height = 2f;
            //characterController.radius = 0.5f;
            hitCollider = false;
        }
    }
}
