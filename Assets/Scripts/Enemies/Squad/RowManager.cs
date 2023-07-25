using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowManager : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 loopStartPos;
    float moveDistance = 0.75f;
    float speed = 2f;
    public bool rightMoveLoop = false;
    public float xPosition = 0.65f;

    //Move To Point
    float lerpTime = 100f;

    //Functions
    private MyFunctions archive;
    internal bool left = false;
    internal bool right = false;

    protected void Start()
    {
        //startPos = transform.position;

        archive = gameObject.GetComponent<MyFunctions>();

        StartCoroutine(StartLerp());
    }

    protected void Update()
    {
        //archive.LoopMovement(transform, startPos, moveDistance, speed, rightMoveLoop);
        endPos = new Vector3(xPosition, transform.position.y, transform.position.z);
    }

    IEnumerator StartLerp()
    {
        yield return new WaitUntil(() => rightMoveLoop);
        Debug.LogWarning(transform.position.y);
        startPos = transform.position;
        loopStartPos = -endPos;
        while (archive.lerpStatus() <= 1)
        {
            transform.position = archive.LerpPosition(startPos, endPos, 1.5f);
            yield return null;
        }
        archive.elapsedTime = 0;
        archive.lerpPercentage = 0;
        StartCoroutine(LoopLerpLeft());
    }

    IEnumerator LoopLerpLeft()
    {
        left = true;
        right = false;
        while (archive.lerpStatus() <= 1)
        {
            if (rightMoveLoop)
            {
                transform.position = archive.LerpPosition(new Vector3(xPosition, transform.position.y, transform.position.z), new Vector3(-xPosition, transform.position.y, transform.position.z), speed);
            }
            yield return null;
        }
        archive.elapsedTime = 0;
        archive.lerpPercentage = 0;
        StartCoroutine(LoopLerpRight());
    }

    IEnumerator LoopLerpRight()
    {
        right = true;
        left = false;
        while (archive.lerpStatus() <= 1)
        {
            if (rightMoveLoop)
            {
                transform.position = archive.LerpPosition(new Vector3(-xPosition, transform.position.y, transform.position.z), new Vector3(xPosition, transform.position.y, transform.position.z), speed);
            }
            yield return null;
        }
        archive.elapsedTime = 0;
        archive.lerpPercentage = 0;
        StartCoroutine(LoopLerpLeft());
    }
}
