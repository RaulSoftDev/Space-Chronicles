using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicAttackButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private fireScript FireScript;

    private void Start()
    {
        FireScript = fireScript.instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        FireScript.canAttack = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        FireScript.canAttack = false;
    }
}
