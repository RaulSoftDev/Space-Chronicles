using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMovementManager : MonoBehaviour
{
    //TESTING
    public float lerpTime = 1000f;
    float currentLerpTime;

    float moveDistance = 100f;

    Vector3 startPos;
    Vector3 endPos;

    public bool startMove = false;

    private MyFunctions archive;

    // Start is called before the first frame update
    void Start()
    {
        archive = gameObject.AddComponent<MyFunctions>();

        startPos = transform.position;
        endPos = transform.position - transform.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        archive.MoveToPoint(transform, startPos, endPos, lerpTime, startMove);
    }
}
