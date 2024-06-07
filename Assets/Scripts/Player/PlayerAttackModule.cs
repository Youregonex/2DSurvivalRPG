using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerAttackModule : MonoBehaviour, IEventListener
{
    [SerializeField] private GameObject _attackPoint;
    [SerializeField] private float _attackRadius = .25f;
    [SerializeField] private float _attackRate = 2f;
    [SerializeField] private int _minAttackValue = 2;
    [SerializeField] private int _maxAttackValue = 5;

    private float _attackCooldown = 0f;
    private readonly float _attackPointOffset = .6f;

    private Weapon _currentWeapon;
    private PlayerAnimation _playerAnimation;
    private PlayerInput _playerInput;
    private PlayerState _playerState;

    private void Awake()
    {
        _playerState = GetComponent<PlayerState>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerInput = GetComponent<PlayerInput>();

        SubscribeToEvents();
    }

    private void Update()
    {
        ManageAttackPointPosition();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void TryStartAttack()
    {
        if (IsPointerOverUIObject())
            return;

        if (Time.time >= _attackCooldown && PlayerCanAttack())
        {
            _playerAnimation.ManageAttackAnimation();
            _attackCooldown = Time.time + 1f / _attackRate;

            Collider2D[] targets = Physics2D.OverlapCircleAll(_attackPoint.transform.position, _attackRadius);

            foreach(Collider2D target in targets)
            {
                if(target.GetComponent<IDamageable>() != null)
                {
                    float damageDone = Random.Range(_minAttackValue, _maxAttackValue + 1);
                    DamageStruct damageStruct = new(damageDone);
                    target.GetComponent<IDamageable>().TakeDamage(damageStruct);
                }
            }
        }
    }

    private void ManageAttackPointPosition()
    {
        if (_playerInput.MovementInput.x != 0)
            _attackPoint.transform.position = new Vector3(transform.position.x + (_attackPointOffset * _playerInput.MovementInput.x),
                                                          transform.position.y,
                                                          _attackPoint.transform.position.z);

        else if (_playerInput.MovementInput.y != 0)
            _attackPoint.transform.position = new Vector3(transform.position.x,
                                                          transform.position.y + (_attackPointOffset * _playerInput.MovementInput.y),
                                                          _attackPoint.transform.position.z);
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

    private bool PlayerCanAttack()
    {
        bool canAttack = true;

        if(_playerState.CurrentPlayerStates.Contains(PlayerStateEnum.Building) ||
           _playerState.CurrentPlayerStates.Contains(PlayerStateEnum.HoldingItem))
        {
            canAttack = false;
        }

        return canAttack;
    }

    public void SubscribeToEvents()
    {
        EventBus.Instance.OnLMBKeyPressed += TryStartAttack;
    }

    public void UnsubscribeFromEvents()
    {
        EventBus.Instance.OnLMBKeyPressed -= TryStartAttack;
    }
}