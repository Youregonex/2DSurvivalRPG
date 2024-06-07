using UnityEngine;

public class BuildingFactory : Factory
{
    private static BuildingFactory instance = null;

    public static BuildingFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(BuildingFactory)) as BuildingFactory;
                if (instance == null)
                    Debug.LogError("SingletoneBase<T>: Could not find GameObject of type " + typeof(BuildingFactory).Name);
            }
            return instance;
        }
    }

    public Building CreateBuilding(BuildingItemDataSO buildingData)
    {
        GameObject newItem = CreateInstance(buildingData);

        Building building = SetUpItemData(newItem, buildingData);

        return building;
    }

    public Building CreateBuildingAtPosition(BuildingItemDataSO buildingData, Vector3 position)
    {
        GameObject newItem = CreateInstanceAtPosition(buildingData, position);

        Building building = SetUpItemData(newItem, buildingData);

        return building;
    }

    public Building CreateBuildingAtPlayerPosition(BuildingItemDataSO buildingData, bool useOffset = false)
    {
        GameObject newItem = CreateInstanceAtPlayerPosition(buildingData, useOffset);

        Building building = SetUpItemData(newItem, buildingData);

        return building;
    }

    private Building SetUpItemData(GameObject newItem, BuildingItemDataSO buildingData)
    {
        Building building = newItem.GetComponent<Building>();

        building.SetBuildingData(buildingData);

        return building;
    }
}
