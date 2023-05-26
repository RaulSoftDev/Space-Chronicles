using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class ParralaxItem : MonoBehaviour
{
    public Vector3 minDirection;
    public Vector3 maxDirection;

    public Vector3 rotationAxis;
    public float rotationSpeed;
    Vector3 direction;

	public float scale = 1;
    float currentScale;//used with the delay

    public float startDelay;
    public GameObject enableAfterDelay;

    void Start()
    {
        direction = new Vector3(Random.Range(minDirection.x, maxDirection.x), Random.Range(minDirection.y, maxDirection.y), Random.Range(minDirection.z, maxDirection.z));

        if(startDelay > 0)
        {
            currentScale = scale;
            scale = 0;
            StartCoroutine(DelayedStart());
        }
    }

    void Update()
    {
        transform.position += direction * Time.deltaTime * scale;

        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelay);
        scale = currentScale;

        if(enableAfterDelay != null)
            enableAfterDelay.SetActive(true);
    }
}
