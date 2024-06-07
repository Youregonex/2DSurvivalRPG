using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
    }

    public void ManageMoveAnimation(Vector2 lastMove, Vector2 movementDirection)
    {
        _playerAnimator.SetFloat("Horizontal", movementDirection.x);
        _playerAnimator.SetFloat("Vertical", movementDirection.y);

        _playerAnimator.SetFloat("CurrentDirX", lastMove.x);
        _playerAnimator.SetFloat("CurrentDirY", lastMove.y);

        _playerAnimator.SetBool("IsMoving", movementDirection.x != 0 || movementDirection.y != 0);
    }

    public void ManageAttackAnimation()
    {
        _playerAnimator.SetTrigger("Attack");
    }

    private void PlayerAttackAnimationStarted() // Used by Animation Event
    {
        EventBus.Instance.AttackAnimationStart();
    }

    private void PlayerAttackAnimationEnded() // Used by Animation Event
    {
        EventBus.Instance.AttackAnimationEnd();
    }
}
