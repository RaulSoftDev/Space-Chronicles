using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlackScreenLoader : Singleton<BlackScreenLoader>
{
    private Transform parent;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        parent = GetComponent<Transform>().parent.transform;

        GameObject[] objP = GameObject.FindGameObjectsWithTag("BSParent");

        if (objP.Length > 1)
        {
            Destroy(parent);
        }

        DontDestroyOnLoad(parent);

        /*GameObject[] objs = GameObject.FindGameObjectsWithTag("BScreen");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);*/
    }

    public void LoadBlackScreen()
    {
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("SetScreenBlackOn");
        }
    }

    public void LoadOutBlackScreen()
    {
        if (GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetTrigger("SetScreenBlackOff");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        parent.GetComponent<Canvas>().worldCamera = Camera.main;
        LoadOutBlackScreen();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
