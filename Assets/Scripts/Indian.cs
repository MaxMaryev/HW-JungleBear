using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Animator), typeof(AudioSource))]
public class Indian : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Collider2D _leftPatrolBorder;
    [SerializeField] private Collider2D _rightPatrolBorder;
    [SerializeField] private Bear _bear;

    private int _hitForce = 15;
    private Coroutine _moveRight, _moveLeft;
    private SpriteRenderer _spriteRenderer;
    private AudioSource _audioSource;
    private Animator _animator;
    private int _attackHash;
    private WaitForSeconds _waitForThreeTenthSecond;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _attackHash = Animator.StringToHash("Attack");
        _waitForThreeTenthSecond = new WaitForSeconds(0.3f);
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
                StartCoroutine(Attack(bear, Vector2.right));
            else
                StartCoroutine(Attack(bear, Vector2.left));
        }
    }

    private IEnumerator Attack(Bear bear, Vector2 hitDirection)
    {
        yield return _waitForThreeTenthSecond;
        _animator.SetTrigger(_attackHash);
        yield return _waitForThreeTenthSecond;
        _audioSource.Play();
        bear.GetComponent<Rigidbody2D>().AddForce((Vector2.up + hitDirection) * _hitForce, ForceMode2D.Impulse);
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
