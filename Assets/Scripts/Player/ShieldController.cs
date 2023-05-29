using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldObject;
    public Slider shieldSlider;
    public Image shieldSliderColor;
    public bool shieldActive = false;
    public Button shieldButton;
    public Image buttonBackground360;

    private void Start()
    {
        StartCoroutine(EnableShield());
    }

    private void Update()
    {
        SaveCurrentShieldValue();
        
        if(shieldActive)
        {
            shieldButton.interactable = false;
        }
    }

    private void SaveCurrentShieldValue()
    {
        //shieldSlider.value = PlayerHealth.instance.playerShieldPoints;
        buttonBackground360.fillAmount = (PlayerHealth.instance.playerShieldPoints / 2) / 10;
    }

    private IEnumerator shieldTime()
    {
        shieldActive = true;
        PlayerHealth.instance.GetDamage = false;
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOn");
        //shieldSlider.GetComponent<Animator>().SetBool("ActivateShieldSlider", true);
        yield return new WaitForSecondsRealtime(10);
        /*shieldSlider.GetComponent<Animator>().SetBool("ActivateShieldSlider", false);
        shieldSliderColor.color = new Color(0, 242, 255);*/
        buttonBackground360.fillAmount = 0;
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
        PlayerHealth.instance.GetDamage = true;
        PlayerHealth.instance.playerShieldPoints = 0;
        shieldActive=false;
    }

    public void TurnShieldOn()
    {
        StartCoroutine(shieldTime());
    }

    IEnumerator EnableShield()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.playerShieldPoints == 20 && !shieldActive);
            shieldButton.interactable = true;
        }
    }
}
