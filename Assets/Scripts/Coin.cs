using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class Coin : MonoBehaviour
{
    [SerializeField] private SpawnCoins _spawnCoins;

    private AudioSource _audioSource;
    private Animator _animator;
    private bool _isTaken;
    private int _beTakenHash = Animator.StringToHash("BeTaken");

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void BeTaken()
    {
        if (_isTaken == false)
            StartCoroutine(DestroyWithEffects());

        IEnumerator DestroyWithEffects()
        {
            _isTaken = true;
            _audioSource.Play();
            _animator.SetTrigger(_beTakenHash);

            yield return new WaitForSeconds(_audioSource.clip.length);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Bear bear))
        {
            BeTaken();
        }
    }
}
