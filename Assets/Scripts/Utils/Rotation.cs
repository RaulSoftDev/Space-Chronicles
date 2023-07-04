using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.forward, 5 * Time.deltaTime);
    }
}
