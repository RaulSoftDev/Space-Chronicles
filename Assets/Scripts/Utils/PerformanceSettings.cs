using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceSettings : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Application.targetFrameRate = 300;
        QualitySettings.vSyncCount = 0;
    }
}
