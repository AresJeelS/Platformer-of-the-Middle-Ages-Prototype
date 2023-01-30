using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float timeToChase = 3f;
    [SerializeField] private float timeToWait = 5f;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float walkDistance = 6f;
    [SerializeField] private Transform enemyModelTransform;

    private Rigidbody2D _rigidbody;
    private Transform _playerTransform;
    private Vector2 _leftBoundaryPosition;
    private Vector2 _nextPoint;
    private Vector2 _rightBoundaryPosition;

    private bool _isChasingPlayer;
    private bool _isFacingRight = true;
    private bool _isWait;
    private bool _collidedWithPlayer;

    private float _chaseTime;
    private float _waitTime;
    private float _walkSpeed;

    public bool IsFacingRight
    {
        get => _isFacingRight;
    }
    public void StartChasingPlayer()
    {
        _isChasingPlayer = true;
        _chaseTime = timeToChase;
        _walkSpeed = chasingSpeed;
    }
    private void Start()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _leftBoundaryPosition = transform.position;
        _rightBoundaryPosition = _leftBoundaryPosition + Vector2.right * walkDistance;
        _waitTime = timeToWait;
        _chaseTime = timeToChase;
        _walkSpeed = patrolSpeed;
    }
    private void Update()
    {
        if (_isChasingPlayer)
        {
            StartChasingTimer();
        }
        if (_isWait && !_isChasingPlayer)
        {
            StartWaitTimer();
        }

        if (ShouldWait())
        {
            _isWait = true;
        }
    }
    private void FixedUpdate()
    {
        _nextPoint = Vector2.right * _walkSpeed * Time.fixedDeltaTime;

        if (_isChasingPlayer && _collidedWithPlayer)
        {
            return;
        }
        if (_isChasingPlayer)
        {
            ChasePlayer();
        }
        if (!_isWait && !_isChasingPlayer)
        {
            Patrol();
        }
    }
    private void Patrol()
    {
        if (!_isFacingRight)
        {
            _nextPoint.x *= -1;
        }
        _rigidbody.MovePosition((Vector2)transform.position + _nextPoint);
    }
    private void ChasePlayer()
    {
        float distance = DistanceToPlayer();
        if (distance < 0)
        {
            _nextPoint.x *= -1;
        }
        if (distance > 0.2f && !_isFacingRight)
        {
            Flip();
        }
        else if (distance < 0.2f && _isFacingRight)
        {
            Flip();
        }
        _rigidbody.MovePosition((Vector2)transform.position + _nextPoint);
    }
    public float DistanceToPlayer()
    {
        return _playerTransform.position.x - transform.position.x;
    }

    private void StartWaitTimer()
    {
        _waitTime -= Time.deltaTime;
        if (_waitTime < 0f)
        {
            _waitTime = timeToWait;
            _isWait = false;
            Flip();
        }
    }

    private void StartChasingTimer()
    {
        _chaseTime -= Time.deltaTime;

        if (_chaseTime < 0f)
        {
            _isChasingPlayer = false;
            _chaseTime = timeToChase;
            _walkSpeed = patrolSpeed;
        }
    }
    private bool ShouldWait()
    {
        bool isOutOfRightBoundary = _isFacingRight && transform.position.x >= _rightBoundaryPosition.x;
        bool isOutOfLeftBoundary = !_isFacingRight && transform.position.x <= _leftBoundaryPosition.x;
        return isOutOfLeftBoundary || isOutOfRightBoundary;
    }
    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 newscale = enemyModelTransform.localScale;
        newscale.x *= -1;
        enemyModelTransform.localScale = newscale;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_leftBoundaryPosition, _rightBoundaryPosition);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerMove player = other.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            _collidedWithPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        PlayerMove player = other.gameObject.GetComponent<PlayerMove>();
        if (player != null)
        {
            _collidedWithPlayer = false;
        }
    }
}
