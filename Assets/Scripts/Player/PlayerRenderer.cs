using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
}
