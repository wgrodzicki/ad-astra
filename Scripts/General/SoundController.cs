using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        TurnOffSound();
    }

    private void TurnOffSound()
    {
        if (_audioSource == null)
            return;

        if (_audioSource.isPlaying)
            return;

        this.gameObject.SetActive(false);
    }
}
