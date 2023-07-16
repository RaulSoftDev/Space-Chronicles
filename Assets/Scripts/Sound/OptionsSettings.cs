using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsSettings : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider buttonsSlider;
    [SerializeField] Slider fxSlider;
    [SerializeField] float currentVolume = 0;

    private void Start()
    {
        SetVolumes();

        LoadMusicVolume();
        LoadButtonsVolume();
        LoadFXVolume();
    }

    public void SetMusicVolume(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetButtonsVolume(float sliderValue)
    {
        mixer.SetFloat("ButtonsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("ButtonsVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetFXVolume(float sliderValue)
    {
        mixer.SetFloat("FXVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("FXVolume", Mathf.Log10(sliderValue) * 20);
    }

    private void LoadMusicVolume()
    {
        mixer.GetFloat("MusicVolume", out currentVolume);
        float linear = Mathf.Pow(10.0f, currentVolume / 20.0f);
        musicSlider.value = linear;
    }

    private void LoadButtonsVolume()
    {
        mixer.GetFloat("ButtonsVolume", out currentVolume);
        float linear = Mathf.Pow(10.0f, currentVolume / 20.0f);
        buttonsSlider.value = linear;
    }

    private void LoadFXVolume()
    {
        mixer.GetFloat("FXVolume", out currentVolume);
        float linear = Mathf.Pow(10.0f, currentVolume / 20.0f);
        fxSlider.value = linear;
    }

    private void SetVolumes()
    {
        mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", Mathf.Log10(0.5f) * 20));
        mixer.SetFloat("ButtonsVolume", PlayerPrefs.GetFloat("ButtonsVolume", Mathf.Log10(0.5f) * 20));
        mixer.SetFloat("FXVolume", PlayerPrefs.GetFloat("FXVolume", Mathf.Log10(0.5f) * 20));
    }
}
