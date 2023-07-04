using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    float moveDistance = 0.75f;
    float speed = 2f;
    public bool rightMoveLoop = false;

    //Move To Point
    float lerpTime = 100f;

    //Functions
    private MyFunctions archive;

    protected void Start()
    {
        archive = gameObject.AddComponent<MyFunctions>();

        startPos = transform.localPosition;
        endPos = new Vector3(0.8f, 0, 0);
    }

    protected void Update()
    {
        archive.LoopMovement(transform, startPos, moveDistance, speed, rightMoveLoop);
    }
}
