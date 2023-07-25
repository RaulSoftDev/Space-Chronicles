using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXScript : MonoBehaviour
{
    public AudioClip alternateClip;

    public void PlaySoundFXOnPlayer(AudioClip clip)
    {
        if (PlayerHealth.instance.GetDamage)
        {
            Debug.LogWarning("Player Damaged");
            PlayerHealth.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Player Damaged with shield");
            if(clip.name == "Shield_End")
            {
                PlayerHealth.instance.gameObject.GetComponent<AudioSource>().volume = 0.25f;
                PlayerHealth.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            }
            else
            {
                PlayerHealth.instance.gameObject.GetComponent<AudioSource>().volume = 0.35f;
                PlayerHealth.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
            }
        }
    }

    public void PlaySoundFXOnEnemies(AudioClip clip)
    {
        Debug.LogWarning("Enemy damaged");
        if (LevelManager.instance.enabled)
        {
            switch (clip.name)
            {
                case "Enemies_Shield_On":
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().volume = 0.85f;
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().clip = clip;
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().Play();
                    break;
                case "Enemies_Shield_Impact":
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().volume = 0.5f;
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
                    break;
                case "Enemies_Shield_Broke":
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().volume = 0.85f;
                    LevelManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
                    break;
                default:
                    break;
            }
        }
        else
        {
            //Tutorial Level
            TutorialManager.instance.currentSquad.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}
