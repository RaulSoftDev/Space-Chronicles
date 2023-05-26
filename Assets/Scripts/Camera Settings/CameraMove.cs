using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float cameraSpeedMove = 1f;
    public float layersOffSet = 0.2f;
    public Transform background;
    public SpriteRenderer BGLayer;
    private Transform map1;
    private Transform map2;

    public GameObject[] layers;

    private void Start()
    {
        map1 = Instantiate(background, background.position, background.rotation);
        background.gameObject.SetActive(false);
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
            yield return new WaitUntil(() => map1 != null && map1.position.y < 0);
            map2 = Instantiate(map1, new Vector3(map1.position.x, map1.position.y + BGLayer.sprite.bounds.size.y, map1.position.z), map1.rotation);

            yield return new WaitUntil(() => map2 != null && map2.position.y < 0);
            map1 = Instantiate(map2, new Vector3(map2.position.x, map2.position.y + BGLayer.sprite.bounds.size.y, map2.position.z), map2.rotation);
        }
    }

    private void MapsManager()
    {
        if (map1 != null)
        {
            map1.Translate(Vector3.up * cameraSpeedMove * Time.deltaTime);
        }

        if (map2 != null)
        {
            map2.Translate(Vector3.up * cameraSpeedMove * Time.deltaTime);
        }

        if (map1 != null && map1.position.y < -11)
        {
            Destroy(map1.gameObject);
        }

        if (map2 != null && map2.position.y < -11)
        {
            Destroy(map2.gameObject);
        }
    }

    private IEnumerator Map3DEffect()
    {
        while (true)
        {
            yield return new WaitUntil(() => map1 != null);
            Vector2 offSetPosition1 = map1.position - transform.position;

            for (int i = 0; i < map1.childCount; i++)
            {
                map1.GetChild(i).transform.Translate(new Vector3(offSetPosition1.x * i, offSetPosition1.y * i, 0), Space.World);
            }
        }
    }

    private void MoveLayers()
    {
        //To be implemented
    }
}
