using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunctions : MonoBehaviour
{
    private float squadLerpTime = 0;
    private float squadElapsedTime = 0;
    private float squadLerpPercentage = 0;

    private float currentLerpTime = 0;
    public float elapsedTime = 0;
    public float lerpPercentage = 0;

    public void MoveToPoint(Transform target, Vector3 startPosition, Vector3 endPosition, float lerpTime, bool startMove)
    {
        if (startMove)
        {
            //increment timer once per frame
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }

            //lerp!
            float perc = currentLerpTime / lerpTime;
            target.position = Vector3.Lerp(startPosition, endPosition, perc);
        }
    }

    public Vector3 SquadLerpPosition(Vector3 startPos, Vector3 endPos, float desiredDuration)
    {
        squadElapsedTime += Time.deltaTime;
        squadLerpPercentage = squadElapsedTime / desiredDuration;
        Vector3 currentPosition = Vector3.Lerp(startPos, endPos, squadLerpPercentage);
        return currentPosition;
    }

    public void ResetLerp()
    {
        squadElapsedTime = 0;
    }

    public void LoopMovement(Transform target, Vector3 startPosition, float maxDistance, float speed, bool startLoop)
    {
        if (startLoop)
        {
            float distance = Mathf.Sin(Time.timeSinceLevelLoad * speed);
            target.localPosition = startPosition + Vector3.right * distance * maxDistance;
        }
    }

    public Vector3 LerpPosition(Vector3 startPos, Vector3 endPos, float desiredDuration)
    {
        elapsedTime += Time.deltaTime;
        lerpPercentage = elapsedTime / desiredDuration;
        Vector3 currentPosition = Vector3.Lerp(startPos, endPos, lerpPercentage);
        return currentPosition;
    }

    public float lerpStatus()
    {
        return lerpPercentage;
    }
}
