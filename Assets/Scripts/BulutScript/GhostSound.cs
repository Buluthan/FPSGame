using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSound : MonoBehaviour
{
    private AudioSource audioSource; // AudioSource bileþeni
    public float minSoundInterval = 5f; // Minimum ses aralýðý
    public float maxSoundInterval = 10f; // Maksimum ses aralýðý

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource bileþenini al

        // Ýlk sesi rastgele bir zaman sonra oynat
        Invoke("PlayGhostSound", Random.Range(minSoundInterval, maxSoundInterval));
    }

    private void PlayGhostSound()
    {
        audioSource.Play(); // Sesi oynat

        // Bir sonraki sesi rastgele bir zaman sonra oynat
        Invoke("PlayGhostSound", Random.Range(minSoundInterval, maxSoundInterval));
    }
}
