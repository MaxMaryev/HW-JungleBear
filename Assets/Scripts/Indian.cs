using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
public class Indian : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider2D _leftPatrolBorder;
    [SerializeField] private Collider2D _rightPatrolBorder;
    [SerializeField] private Bear _bear;

    private int _hitForce = 15;
    private Rigidbody2D _rigidbody;
    private Coroutine _moveRight;
    private Coroutine _moveLeft;
    private Coroutine _leftAttack;
    private Coroutine _rightAttack;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private Animator _animator;
    private int _attackHash;
    private int _dieHash;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _attackHash = Animator.StringToHash("Attack");
        _dieHash = Animator.StringToHash("Die");
        _wait = new WaitForSeconds(0.3f);
    }

    public void Die()
    {
        TryStopCoroutine(_moveLeft);
        TryStopCoroutine(_moveRight);
        TryStopCoroutine(_leftAttack);
        TryStopCoroutine(_rightAttack);

        _animator.SetTrigger(_dieHash);
        GetComponent<BoxCollider2D>().enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Static;
    }

    private void TryStopCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }

    private void OnTriggerEnter2D(Collider2D borderPoint)
    {
        if (borderPoint == _leftPatrolBorder)
        {
            Stop(_moveLeft);

            _moveRight = StartCoroutine(Move(Vector3.right, true));
        }
        else if (borderPoint == _rightPatrolBorder)
        {
            Stop(_moveRight);

            _moveLeft = StartCoroutine(Move(Vector3.left, false));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Bear bear))
        {
            if (_spriteRenderer.flipX == true)
                _rightAttack = StartCoroutine(Attack(bear, Vector2.right));
            else
                _leftAttack = StartCoroutine(Attack(bear, Vector2.left));
        }
    }

    private IEnumerator Attack(Bear bear, Vector2 hitDirection)
    {
        yield return _wait;
        _animator.SetTrigger(_attackHash);
        yield return _wait;
        _audioSource.Play();
        bear.Rigidbody.AddForce((Vector2.up + hitDirection) * _hitForce, ForceMode2D.Impulse);
    }

    private IEnumerator Move(Vector3 direction, bool flip)
    {
        bool isPatrolling = true;

        _spriteRenderer.flipX = flip;

        while (isPatrolling)
        {
            transform.position += direction * _speed * Time.deltaTime;
            yield return null;
        }
    }

    private void Stop(Coroutine move)
    {
        if (move != null)
            StopCoroutine(move);
    }
}
