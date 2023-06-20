using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    Vector3 startPos;
    float moveDistance = 0.75f;
    float speed = 2f;
    public bool rightMoveLoop = false;

    protected void Start()
    {
        startPos = transform.localPosition;
    }

    protected void Update()
    {
        if (rightMoveLoop)
        {
            float distance = Mathf.Sin(Time.timeSinceLevelLoad * speed);
            transform.localPosition = startPos + Vector3.right * distance * moveDistance;
        }
    }
}
