using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
    [Header("SHIELD INSTANCE")]
    [SerializeField] private GameObject shieldObject;

    [Header("SHIELD IMAGES")]
    [SerializeField] internal Button shieldButton;
    [SerializeField] private Image buttonBackground360;
    [SerializeField] private Sprite buttonOnMode;
    [SerializeField] private Sprite buttonOffMode;

    [SerializeField] private AudioClip activateShield;
    [SerializeField] private AudioClip shieldOn;

    //Booleans
    internal bool shieldActive = false;

    //Floats
    private float sliderValue = 10;

    private void Start()
    {
        StartCoroutine(EnableShield());
    }

    private void Update()
    {
        SaveCurrentShieldValue();
        DecreaseShieldValueOverTime();
        ShieldOnState();
        DisableShieldOnDeath();
    }

    private void ShieldOnState()
    {
        if (shieldActive)
        {
            shieldButton.interactable = false;
            shieldButton.image.sprite = buttonOffMode;
        }
    }

    private void DisableShieldOnDeath()
    {
        if (PlayerHealth.instance.playerHealth <= 0)
        {
            StopAllCoroutines();
            shieldButton.interactable = false;
            shieldButton.image.sprite = buttonOffMode;
        }
    }

    private void SaveCurrentShieldValue()
    {
        if (!shieldActive)
        {
            buttonBackground360.fillAmount = (PlayerHealth.instance.playerShieldPoints / 2) / 10;
        }
    }

    private void DecreaseShieldValueOverTime()
    {
        if (shieldActive)
        {
            sliderValue = Mathf.Clamp(sliderValue - 1 * Time.deltaTime, 0, 10);
            buttonBackground360.fillAmount = sliderValue / 10;
        }
    }

    private IEnumerator shieldTime()
    {
        PlayerHealth.instance.GetDamage = false;
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOn");
        gameObject.GetComponent<AudioSource>().volume = 0.75f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(shieldOn);
        yield return new WaitForSecondsRealtime(10);
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

    public void TurnOffShield()
    {
        shieldObject.GetComponent<Animator>().SetTrigger("ShieldOff");
    }

    IEnumerator EnableShield()
    {
        yield return new WaitUntil(() => PlayerHealth.instance.playerShieldPoints == 20 && !shieldActive);
        gameObject.GetComponent<AudioSource>().volume = 0.75f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(activateShield);
        shieldButton.interactable = true;
        shieldButton.image.sprite = buttonOnMode;
        yield return new WaitUntil(() => shieldActive);
        StartCoroutine(EnableShield());
    }
}
