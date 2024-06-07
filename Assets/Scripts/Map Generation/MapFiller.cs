using UnityEngine;
using System.Collections.Generic;

public class MapFiller : MonoBehaviour
{
    public MapGeneratorCA _mapGenerator;

    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Range(36, 70)]
    [SerializeField] private int _wallFillPercent;

    [Range(1, 10)]
    [SerializeField] private int _smootheGenerations;

    [Range(0, 100)]
    [SerializeField] private int _treeFillPercent;

    [SerializeField] private string _seed;

    [SerializeField] private bool _useRandomSeed;
    [SerializeField] private bool _generateIslands;

    [SerializeField] private Sprite[] _grassSprites;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private Sprite[] _treeSprites;
    [SerializeField] private Sprite _wallSprite;
    [SerializeField] private GameObject _floorPrefab;
    [SerializeField] private GameObject _obstaclePrefab;

    private bool tilesFilled = false;
    private GameObject _tilesParent;
    private string _tilesParentName = "Tiles";

    public Dictionary<Vector2Int, TerrainTile> Tiles { get; private set; } = new();

    private void Start()
    {
        ReCreateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ReCreateMap();
        }
    }

    private void FillTiles()
    {
        if (tilesFilled)
            return;

        _tilesParent = new(_tilesParentName);
        _tilesParent.transform.position = Vector3.zero;

        Sprite tileSprite;
        GameObject trees = new(nameof(trees));

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                switch(_mapGenerator.Grid[x, y])
                {
                    case 0:
                        tileSprite = _grassSprites[Random.Range(0, _grassSprites.Length)];
                        Tiles[new Vector2Int(x, y)] = PlaceTile(tileSprite, _floorPrefab, new Vector3(x, y), _tilesParent);
                        break;

                    case 1:
                        tileSprite = _wallSprite;
                        Tiles[new Vector2Int(x, y)] = PlaceTile(tileSprite, _obstaclePrefab, new Vector3(x, y - .5f), _tilesParent);
                        break;

                    case 2:
                        tileSprite = _grassSprites[Random.Range(0, _grassSprites.Length)];
                        Tiles[new Vector2Int(x, y)] = PlaceTile(tileSprite, _floorPrefab, new Vector3(x, y), _tilesParent);
                        PlaceTree(new Vector3(x, y), _treePrefab, trees);
                        break;
                }
            }
        }

        tilesFilled = true;
    }

    private TerrainTile PlaceTile(Sprite sprite, GameObject tilePrefab, Vector3 position, GameObject parent)
    {
        TerrainTile tile = Instantiate(tilePrefab, position, Quaternion.identity).GetComponent<TerrainTile>();
        tile.gameObject.name = $"Tile {position.x} | {position.y}";
        tile.transform.parent = parent.transform;
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        return tile;
    }

    private GameObject PlaceTree(Vector3 position, GameObject prefab, GameObject parent)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.parent = parent.transform;
        obj.transform.name = $"Tree {position.x} | {position.y}";
        SpriteRenderer treeSprite = obj.GetComponent<SpriteRenderer>();
        treeSprite.sprite = _treeSprites[Random.Range(0, _treeSprites.Length)];

        return obj;
    }

    private void ReCreateMap()
    {
        DestroyTiles();
        
        _mapGenerator = new MapGeneratorCA(_width, _height, _wallFillPercent, _smootheGenerations, _treeFillPercent, _seed, _useRandomSeed, _generateIslands);
        FillTiles();
    }

    private void DestroyTiles()
    {
        GameObject parent = GameObject.Find("Tiles");

        if (parent == null)
            return;

            Destroy(parent);

        tilesFilled = false;
    }
}
