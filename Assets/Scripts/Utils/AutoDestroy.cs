using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float secondsToDestroy = 1.6f;

    private void Start()
    {
        Destroy(gameObject, secondsToDestroy);
    }
}
