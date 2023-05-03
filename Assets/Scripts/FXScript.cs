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
            PlayerHealth.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }

    public void PlaySoundFXOnEnemies(AudioClip clip)
    {
        Debug.LogWarning("Enemy damaged");
        if (clip.name == "EnemiesShield")
        {
            EnemySquadMove.instance.gameObject.GetComponent<AudioSource>().clip = clip;
            EnemySquadMove.instance.gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            EnemySquadMove.instance.gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }
    }
}
