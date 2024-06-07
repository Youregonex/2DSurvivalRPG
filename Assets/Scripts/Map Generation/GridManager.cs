using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;


    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }
}
