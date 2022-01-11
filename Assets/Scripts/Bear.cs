using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Rigidbody2D), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Bear : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private AudioSource _audioSource;
    private int _hitForce = 5;
    private int _speedHash = Animator.StringToHash("Speed");
    private int _attackHash = Animator.StringToHash("Attack");
    private int _jumpHash = Animator.StringToHash("Jump");
    private int _dieHash = Animator.StringToHash("Die");
    private Vector3 _leftBorderPosition, _rightBorderPosition;
    private int _coinsCount = 0;

    private void Awake()
    {
        float leftBorderX = -22;
        float leftBorderY = 1.9165f;
        float rightBorderX = 128;
        float rightBorderY = 7.7475f;

        _leftBorderPosition = new Vector3(leftBorderX, leftBorderY);
        _rightBorderPosition = new Vector3(rightBorderX, rightBorderY);

        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        Jump();
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            Debug.Log(++_coinsCount);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Indian indian))
        {
            if (_spriteRenderer.flipX == true)
                StartCoroutine(Attack(indian, Vector2.left));
            else
                StartCoroutine(Attack(indian, Vector2.right));
        }
    }

    private IEnumerator Attack(Indian indian, Vector2 hitDirection)
    {
        if (Input.GetKey(KeyCode.F))
        {
            _animator.SetTrigger(_attackHash);
            yield return null;
            _audioSource.Play();
            indian.GetComponent<Rigidbody2D>().AddForce((Vector2.up + hitDirection) * _hitForce, ForceMode2D.Impulse);
            Kill(indian);
        }
    }

    private void Kill(Indian indian)
    {
        indian.StopAllCoroutines();
        indian.GetComponent<Animator>().SetTrigger(_dieHash);
        indian.GetComponent<BoxCollider2D>().enabled = false;
        indian.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(_rigidbody.velocity.y) < 0.05f)
        {
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _animator.SetTrigger(_jumpHash);
        }
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.A))
            ChangePosition(Vector3.left);
        else if (Input.GetKey(KeyCode.D))
            ChangePosition(Vector3.right);
        else
            _animator.SetFloat(_speedHash, 0);

        TryRun();
        StopAtBorder();

        void TryRun()
        {
            int speedBooster = 2;

            if (Input.GetKeyDown(KeyCode.LeftShift))
                _speed = _speed * speedBooster;
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                _speed = _speed / speedBooster;
        }

        void StopAtBorder()
        {
            if (transform.position.x < _leftBorderPosition.x)
                transform.position = _leftBorderPosition;
            else if (transform.position.x > _rightBorderPosition.x)
                transform.position = _rightBorderPosition;
        }
    }

    private void ChangePosition(Vector3 direction)
    {
        transform.position += direction * _speed * Time.deltaTime;
        _animator.SetFloat(_speedHash, _speed);

        if (direction == Vector3.right)
            _spriteRenderer.flipX = false;
        else
            _spriteRenderer.flipX = true;
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.F))
            _animator.SetTrigger(_attackHash);
    }
}
