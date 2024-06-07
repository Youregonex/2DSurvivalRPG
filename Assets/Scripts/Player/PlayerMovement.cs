using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayer, IEventListener
{
    [SerializeField] private float _moveSpeed;

    private Vector2 _movementDirection;
    private Rigidbody2D _rigidBody;
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    private bool _canMove = true;
    private Vector2 _lastDirection;

    public Vector2 LastDirection => _lastDirection;


    private void Awake()
    {
        SubscribeToEvents();
    }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void Update()
    {
        UpdateInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateInput()
    {
        if (!_canMove)
        {
            _movementDirection = Vector2.zero;
        }
        else
        {
            _movementDirection = _playerInput.MovementInput.normalized;
        }

        _lastDirection = _movementDirection != Vector2.zero ? _movementDirection : _lastDirection;
    }

    private void MovePlayer()
    {
        _rigidBody.velocity = new Vector2(_movementDirection.x, _movementDirection.y) * _moveSpeed;
        _playerAnimation.ManageMoveAnimation(_lastDirection, _movementDirection);
    }

    private void StopMovement() => _canMove = false;

    private void ContinueMovement() => _canMove = true;

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnAttackAnimationStart += StopMovement;
        EventBus.Instance.OnAttackAnimationEnd += ContinueMovement;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnAttackAnimationStart -= StopMovement;
        EventBus.Instance.OnAttackAnimationEnd -= ContinueMovement;
    }
}
