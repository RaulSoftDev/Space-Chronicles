using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneShortcut : MonoBehaviour
{
    private MenuScript menuManager;

    private void Start()
    {
        menuManager = MenuScript.Instance;
    }

    public void LoadScene(int sceneID)
    {
        menuManager.StartMenu(sceneID);
    }

    public void RestartLastScene()
    {
        menuManager.RestartPreviousScene();
    }
}
