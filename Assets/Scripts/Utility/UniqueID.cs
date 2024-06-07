using System;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id = Guid.NewGuid().ToString();
    [SerializeField] private static SerializableDictionary<string, GameObject> _idDatabase = new();

    public string ID => _id;


    private void OnValidate()
    {
        if (_idDatabase.ContainsKey(_id))
            Generate();
        else _idDatabase.Add(_id, this.gameObject);
    }

    private void OnDestroy()
    {
        if (_idDatabase.ContainsKey(_id))
            _idDatabase.Remove(_id);
    }

    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        _idDatabase.Add(_id, this.gameObject);
    }
}
