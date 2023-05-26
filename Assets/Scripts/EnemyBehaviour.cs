using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    public static EnemyBehaviour instance;

    public bool enemyInPosition = false;

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
        yield return new WaitForSeconds(2f);
        float counter = 0f;
        while (t.position.y >= 2.1f)
        {
            t.position = Vector3.Lerp(new Vector3(t.position.x, t.position.y, t.position.z), new Vector3(t.position.x, 2f, t.position.z), counter / 3 * Time.deltaTime);
            counter += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Enemy Ready");
        enemyInPosition = true;
        StartCoroutine(VerticalMovement(t));
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
                target.transform.localPosition = Vector3.Lerp(new Vector3(-1f, target.transform.localPosition.y, target.transform.localPosition.z), new Vector3(1f, target.transform.localPosition.y, target.transform.localPosition.z), counter / 1);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            counter = 0f;
            while (counter < 1)
            {
                target.transform.localPosition = Vector3.Lerp(new Vector3(1f, target.transform.localPosition.y, target.transform.localPosition.z), new Vector3(-1f, target.transform.localPosition.y, target.transform.localPosition.z), counter / 1);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator VerticalMovement(Transform t)
    {
        Vector3 position1 = t.transform.position;
        Vector3 position2 = new Vector3(t.position.x, 0, t.position.z);

        yield return new WaitUntil(() => enemyInPosition);
        Debug.Log("Vertical");
        while (Application.isPlaying)
        {
            float counter = 0f;
            while (counter < 2)
            {
                t.position = Vector3.Lerp(position1, position2, counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
            counter = 0f;
            while (counter < 2)
            {
                t.position = Vector3.Lerp(position2, position1, counter / 2);
                counter += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
