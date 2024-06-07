using UnityEngine;
using System.Collections;

public class MoveToRandomPositionAround : MonoBehaviour
{
    private float _speed = 4f;
    private float _minOffset = -.7f;
    private float _maxOffset = .7f;

    private void OnEnable()
    {
        Vector3 offset = new Vector3(Random.Range(_minOffset, _maxOffset), Random.Range(_minOffset, _maxOffset) + 1, transform.position.z);

        Vector3 newPosition = transform.position + offset;

        StartCoroutine(MoveToTargetPosition(newPosition));
    }

    private IEnumerator MoveToTargetPosition(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _speed);
            yield return null;
        }
    }

    public void ModifyOffset(float minValue, float maxValue)
    {
        if (minValue >= maxValue)
            maxValue = minValue + 1f;

        _minOffset = minValue;
        _maxOffset = maxValue;
    }

    public void ModifySpeed(float speed)
    {
        _speed = speed;
    }
}