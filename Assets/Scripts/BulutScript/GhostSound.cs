using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSound : MonoBehaviour
{
    private AudioSource audioSource; // AudioSource bile�eni
    public float minSoundInterval = 5f; // Minimum ses aral���
    public float maxSoundInterval = 10f; // Maksimum ses aral���

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // AudioSource bile�enini al

        // �lk sesi rastgele bir zaman sonra oynat
        Invoke("PlayGhostSound", Random.Range(minSoundInterval, maxSoundInterval));
    }

    private void PlayGhostSound()
    {
        audioSource.Play(); // Sesi oynat

        // Bir sonraki sesi rastgele bir zaman sonra oynat
        Invoke("PlayGhostSound", Random.Range(minSoundInterval, maxSoundInterval));
    }
}
