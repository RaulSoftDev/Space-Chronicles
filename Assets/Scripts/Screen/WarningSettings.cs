using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSettings : MonoBehaviour
{
    public void PlaySound(AudioClip clip)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
