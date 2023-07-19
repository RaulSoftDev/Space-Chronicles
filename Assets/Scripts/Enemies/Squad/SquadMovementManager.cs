using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadMovementManager : MonoBehaviour
{
    //TESTING
    public float lerpTime = 1000f;
    float currentLerpTime;

    float moveDistance = 100f;

    public Vector3 startPos;
    public Vector3 currentPos;
    Vector3 endPos;

    public bool startMove = true;
    public bool startMove2 = false;

    private MyFunctions archive;

    // Start is called before the first frame update
    void Start()
    {
        archive = gameObject.AddComponent<MyFunctions>();

        startPos = transform.position;
        endPos = transform.position - transform.up * moveDistance;

        StartCoroutine(ResetLerp());
    }

    // Update is called once per frame
    void Update()
    {
        //archive.MoveToPoint(transform, startPos, endPos, lerpTime, startMove);
        if (startMove)
        {
            transform.position = archive.SquadLerpPosition(startPos, new Vector3(0, -5, 0), lerpTime);
        }

        /*if (startMove2)
        {
            transform.position = archive.SquadLerpPosition(currentPos, new Vector3(0, -5, 0), lerpTime);
        }*/
    }

    IEnumerator ResetLerp()
    {
        yield return new WaitUntil(() => !startMove);
        archive.ResetLerp();
    }
}
