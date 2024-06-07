using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _smootheTime = .25f;
    [SerializeField] private Transform _target;

    private Vector3 _velocity = Vector3.zero;
    private Vector3 _cameraOffset = new(0, 0, -10);

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (_target == null)
            return;
        
        Vector3 targetPosition = _target.position + _cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position,targetPosition, ref _velocity, _smootheTime);
    }
}
