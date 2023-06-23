using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject objectActive;

    //Set the cursor on lock mode (cannot leave the game window)
    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        DontDestroyOnLoad(gameObject);
    }
}
