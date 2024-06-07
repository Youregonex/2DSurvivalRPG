using UnityEngine;

public abstract class Factory : MonoBehaviour
{
    [SerializeField] protected PlayerMovement _player;

    protected GameObject CreateInstance(ItemDataSO objectData)
    {
        GameObject newInstance = Instantiate(objectData.ItemPrefab);

        return newInstance;
    }

    protected GameObject CreateInstanceAtPosition(ItemDataSO objectData, Vector3 position)
    {
        GameObject newInstance = Instantiate(objectData.ItemPrefab, position, Quaternion.identity);

        return newInstance;
    }

    protected GameObject CreateInstanceAtPlayerPosition(ItemDataSO objectData, bool useOffset = false)
    {
        Vector3 targetPosition = useOffset ? GetPlayerPosition() : GetPlayerPosition() + GetPlayerDirection();

        GameObject newInstance = Instantiate(objectData.ItemPrefab, targetPosition, Quaternion.identity);

        return newInstance;
    }
    
    protected Vector2 GetPlayerDirection() => _player.LastDirection;
    protected Vector2 GetPlayerPosition() => _player.transform.position;
}