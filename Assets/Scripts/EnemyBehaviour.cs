using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public static EnemyBehaviour instance;

    public bool enemyVertical = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public IEnumerator SetUpEnemies(Transform t)
    {
        yield return new WaitForSeconds(3f);
        float counter = 0f;
        while (counter < 8)
        {
            t.position = Vector3.Lerp(new Vector3(t.position.x, 7.06f, t.position.z), new Vector3(t.position.x, 1.8f, t.position.z), counter / 8);
            counter += Time.deltaTime;
            yield return null;
        }
        enemyVertical = true;
    }

    public IEnumerator WaitCoroutine(Transform t)
    {
        foreach (Transform child in t)
        {
            StartCoroutine(MoveBetweenPoints(child.transform));
            yield return new WaitForSeconds(0.4f);
        }
    }

    //Move from point A to point B in a loop
    private IEnumerator MoveBetweenPoints(Transform target)
    {
        while (Application.isPlaying)
        {
            float counter = 0f;
            while (counter < 1)
            {
                target.transform.position = Vector3.Lerp(new Vector3(1.10f, target.transform.position.y, target.transform.position.z), new Vector3(2.75f, target.transform.position.y, target.transform.position.z), counter / 1);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            counter = 0f;
            while (counter < 1)
            {
                target.transform.position = Vector3.Lerp(new Vector3(2.75f, target.transform.position.y, target.transform.position.z), new Vector3(1.10f, target.transform.position.y, target.transform.position.z), counter / 1);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
