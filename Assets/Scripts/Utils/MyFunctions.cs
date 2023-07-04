using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunctions : MonoBehaviour
{
    private float currentLerpTime = 0;

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

    public void LoopMovement(Transform target, Vector3 startPosition, float maxDistance, float speed, bool startLoop)
    {
        if (startLoop)
        {
            float distance = Mathf.Sin(Time.timeSinceLevelLoad * speed);
            target.localPosition = startPosition + Vector3.right * distance * maxDistance;
        }
    }
}
