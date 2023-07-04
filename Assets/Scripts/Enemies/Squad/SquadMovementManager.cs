using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMovementManager : MonoBehaviour
{
    //TESTING
    float lerpTime = 100f;
    float currentLerpTime;

    float moveDistance = 10f;

    Vector3 startPos;
    Vector3 endPos;

    public bool startMove = false;

    private MyFunctions archive = new MyFunctions();

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position - transform.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        archive.MoveToPoint(transform, startPos, endPos, lerpTime, startMove);
    }
}
