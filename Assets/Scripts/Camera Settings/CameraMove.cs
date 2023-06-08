using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float cameraSpeedMove = 1f;
    public float layersOffSet = 0.2f;
    public Transform background;
    public Transform background2;
    public SpriteRenderer BGLayer;

    public GameObject[] layers;

    private void Start()
    {
        //map1 = Instantiate(background, background.position, background.rotation);
        //background.gameObject.SetActive(false);
        StartCoroutine(GenerateMap());
        //StartCoroutine(Map3DEffect());
    }

    // Update is called once per frame
    void Update()
    {
        MapsManager();
    }

    private IEnumerator GenerateMap()
    {
        while (true)
        {
            yield return new WaitUntil(() => background != null && background.position.y < 0);
            //background2 = Instantiate(background, new Vector3(background.position.x, background.position.y + background.GetComponent<SpriteRenderer>().bounds.size.y, background.position.z), background.rotation);
            background2.transform.position = new Vector3(background.position.x, background.position.y + background.GetComponent<SpriteRenderer>().bounds.size.y, background.position.z);
            background2.gameObject.SetActive(true);

            yield return new WaitUntil(() => background2 != null && background2.position.y < 0);
            background.transform.position = new Vector3(background2.position.x, background2.position.y + background2.GetComponent<SpriteRenderer>().bounds.size.y, background2.position.z);
            background.gameObject.SetActive(true);
        }
    }

    private void MapsManager()
    {
        if (background != null)
        {
            background.Translate(Vector3.up * cameraSpeedMove * Time.deltaTime);
        }

        if (background2 != null)
        {
            background2.Translate(Vector3.up * cameraSpeedMove * Time.deltaTime);
        }

        if (background != null && background.position.y < -10.3f)
        {
           background.gameObject.SetActive(false);
        }

        if (background2 != null && background2.position.y < -10.3f) 
        {
            background2.gameObject.SetActive(false);
        }
    }

    private IEnumerator Map3DEffect()
    {
        while (true)
        {
            yield return new WaitUntil(() => background != null);
            Vector2 offSetPosition1 = background.position - transform.position;

            for (int i = 0; i < background.childCount; i++)
            {
                background.GetChild(i).transform.Translate(new Vector3(offSetPosition1.x * i, offSetPosition1.y * i, 0), Space.World);
            }
        }
    }

    private void MoveLayers()
    {
        //To be implemented
    }
}
