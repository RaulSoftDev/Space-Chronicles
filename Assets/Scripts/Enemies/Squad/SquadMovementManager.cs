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

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        endPos = transform.position - transform.up * moveDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //reset when we press spacebar
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            currentLerpTime = 0f;
        }*/

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
            transform.position = Vector3.Lerp(startPos, endPos, perc);
        }
    }
}
