using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
