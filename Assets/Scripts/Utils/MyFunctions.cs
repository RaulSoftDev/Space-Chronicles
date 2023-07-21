using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFunctions : MonoBehaviour
{
    private float squadLerpTime = 0;
    private float squadElapsedTime = 0;
    private float squadLerpPercentage = 0;

    private float currentLerpTime = 0;
    internal float elapsedTime = 0;
    internal float lerpPercentage = 0;

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
