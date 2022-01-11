using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _hoverSound;

    private void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound);
    }

    private void PlayHoverSound()
    {
        _audioSource.PlayOneShot(_hoverSound);
    }
}
