using System.Collections;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private float _flashDuration = .125f;

    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;

    private Coroutine _flashCoroutine;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }

    private IEnumerator FlashCoroutine()
    {
        _spriteRenderer.material = _flashMaterial;

        yield return new WaitForSeconds(_flashDuration);

        _spriteRenderer.material = _originalMaterial;

        _flashCoroutine = null;
    }

    public void QuickFlash()
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashCoroutine());
    }

}
