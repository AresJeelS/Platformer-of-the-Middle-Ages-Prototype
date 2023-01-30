using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speedX = 4f;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerModelTransform;
    [SerializeField] private AudioSource jumpSound;

    private LeverArm _leverArm;
    private Finish _finish;
    private Rigidbody2D _rigidbody;

    private float _moveHorizontal;
    private const float _speedMultiply = 50f;
    private bool _isJump;
    private bool _isGround;
    private bool _isFacingRight = true;
    private bool _isFinish;
    private bool _isLeverArm;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _finish = GameObject.FindGameObjectWithTag("Finish").GetComponent<Finish>();
        _leverArm = FindObjectOfType<LeverArm>();

    }
    void Update()
    {
        _animator.SetFloat("speedX", Mathf.Abs(_moveHorizontal));
        _moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) && _isGround)
        {
            _isGround = false;
            _isJump = true;
            jumpSound.Play();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isFinish)
            {
                _finish.FinishLevel();
            }
            if (_isLeverArm)
            {
                _leverArm.ActivateLeverArm();
            }
        }



    }
    void FixedUpdate()
    {
        if (_isJump)
        {
            _rigidbody.AddForce(new Vector2(0f, 380f));
            _isJump = false;
        }
        _rigidbody.velocity = new Vector2(_moveHorizontal * speedX * _speedMultiply * Time.fixedDeltaTime, _rigidbody.velocity.y);

        if (_moveHorizontal > 0f && !_isFacingRight)
        {
            Flip();
        }
        else if (_moveHorizontal < 0f && _isFacingRight)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }
    void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 newscale = playerModelTransform.localScale;
        newscale.x *= -1;
        playerModelTransform.localScale = newscale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();

        if (other.CompareTag("Finish"))
        {
            _isFinish = true;
        }
        if (leverArmTemp != null)
        {
            _isLeverArm = true;
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        LeverArm leverArmTemp = other.GetComponent<LeverArm>();

        if (other.CompareTag("Finish"))
        {
            _isFinish = false;
        }
        if (leverArmTemp != null)
        {
            _isLeverArm = false;
        }
    }
}
