using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnitionScript : MonoBehaviour
{
    public bool ignitionState = false;
    public AudioClip engineStartSound;

    public void PlayAudioStartEngine()
    {
        AudioManager.instance.PlaySoundFXClip(engineStartSound);
    }

    void Update()
    {
        if (ignitionState)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, -24.011f);
            transform.localPosition = new Vector3(-0.733f, 1.332f, 0.9847792f);
        } else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition = new Vector3(-0.7392297f, 1.317522f, 0.9847792f);
        }
    }
}
