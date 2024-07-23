using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAudio : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip_door_open;

    public void PlayDoorOpen()
    {
        source.PlayOneShot(clip_door_open);
    }
}
