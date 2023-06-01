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
    public Sprite buttonOnMode;
    public Sprite buttonOffMode;

    private float sliderValue = 10;

    private void Start()
    {
        StartCoroutine(EnableShield());
    }

    private void Update()
    {
        SaveCurrentShieldValue();
        DecreaseShieldValueOverTime();
        
        if(shieldActive)
        {
            shieldButton.interactable = false;
            shieldButton.image.sprite = buttonOffMode;
        }
    }

    private void SaveCurrentShieldValue()
    {
        if (!shieldActive)
        {
            //shieldSlider.value = PlayerHealth.instance.playerShieldPoints;
            buttonBackground360.fillAmount = (PlayerHealth.instance.playerShieldPoints / 2) / 10;
        }
    }

    private void DecreaseShieldValueOverTime()
    {
        if (shieldActive)
        {
            sliderValue = Mathf.Clamp(sliderValue - 1 * Time.deltaTime, 0, 10);
            buttonBackground360.fillAmount = sliderValue / 10;
            Debug.Log(sliderValue);
        }
    }

    private IEnumerator shieldTime()
    {
        PlayerHealth.instance.GetDamage = false;
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOn");
        //shieldSlider.GetComponent<Animator>().SetBool("ActivateShieldSlider", true);
        yield return new WaitForSecondsRealtime(10);
        /*shieldSlider.GetComponent<Animator>().SetBool("ActivateShieldSlider", false);
        shieldSliderColor.color = new Color(0, 242, 255);*/
        //buttonBackground360.fillAmount = 0;
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
        PlayerHealth.instance.GetDamage = true;
        PlayerHealth.instance.playerShieldPoints = 0;
        shieldActive=false;
    }

    public void TurnShieldOn()
    {
        sliderValue = 10;
        shieldActive = true;
        Debug.Log("Shield ON");
        StartCoroutine(shieldTime());
    }

    IEnumerator EnableShield()
    {
        while (true)
        {
            yield return new WaitUntil(() => PlayerHealth.instance.playerShieldPoints == 20 && !shieldActive);
            shieldButton.interactable = true;
            shieldButton.image.sprite = buttonOnMode;
        }
    }
}
