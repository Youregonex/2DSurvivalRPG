public class MapGeneratorCA
{
    private bool _treesFilled;
    private readonly int _amountOfWallsToChangeTile = 4;

    private readonly int _width;
    private readonly int _height;

    public int[,] Grid { get; private set; }

    public string seed;

    public MapGeneratorCA(int width, int height, int wallFillPercent, int smootheGenerations, int treeFillPercent, string seed, bool useRandomSeed, bool generateIslands)
    {
        _width = width;
        _height = height;
        this.seed = seed;

        Grid = GenerateGrid(wallFillPercent, smootheGenerations, treeFillPercent, useRandomSeed, generateIslands);
    }

    private int[,] GenerateGrid(int wallFillPercent, int smootheGenerations, int treeFillPercent, bool useRandomSeed, bool generateIslands)
    {
        Grid = new int[_width, _height];
        _treesFilled = false;
        RandomFillGrid(useRandomSeed, wallFillPercent);
        SmootheGrid(smootheGenerations, generateIslands, treeFillPercent);

        if(generateIslands)
            PutWallsOnSides();

        return Grid;
    }

    private void RandomFillGrid(bool useRandomSeed, int wallFillPercent)
    {
        if (useRandomSeed)
        {
            seed = UnityEngine.Random.Range(float.MinValue, float.MaxValue).ToString();
        }

        System.Random pseudoRandom = new(seed.GetHashCode());

        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                Grid[x, y] = (pseudoRandom.Next(0, 100) < wallFillPercent) ? 1 : 0;
            }
        }
    }

    private void PutWallsOnSides()
    {
        if (Grid == null)
            return;

        for (int x = 0; x < _width; x++)
        {
            if (Grid[x, _height - 1] != 1)
                Grid[x, _height - 1] = 1;

            if (Grid[x, 0] != 1)
                Grid[x, 0] = 1;
        }

        for (int y = 0; y < _height; y++)
        {
            if (Grid[_width - 1, y] != 1)
                Grid[_width - 1, y] = 1;

            if (Grid[0, y] != 1)
                Grid[0, y] = 1;
        }
    }

    private void SmootheGrid(int smootheGenerations, bool generateIslands, int treeFillPercent)
    {
        for (int i = 0; i < smootheGenerations; i++)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                for (int y = 0; y < Grid.GetLength(1); y++)
                {
                    int neighbourWallTiles = GetSurroundingWallsCount(x, y, generateIslands);

                    if (neighbourWallTiles > _amountOfWallsToChangeTile)
                    {
                        Grid[x, y] = 1;
                    }
                    else if (neighbourWallTiles < _amountOfWallsToChangeTile)
                    {
                        Grid[x, y] = 0;
                    }
                }
            }
        }

        _treesFilled = false;
        RandomTreeFill(treeFillPercent);
    }

    private int GetSurroundingWallsCount(int x, int y, bool generateIslands)
    {
        int wallCount = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < Grid.GetLength(0) && neighbourY >= 0 && neighbourY < Grid.GetLength(1))
                {
                    if (neighbourX != x || neighbourY != y)
                    {
                        if (Grid[neighbourX, neighbourY] == 1)
                            wallCount += Grid[neighbourX, neighbourY];
                    }
                }
                else
                {
                    if (generateIslands)
                        wallCount++;
                }
            }
        }

        return wallCount;
    }

    private void RandomTreeFill(int treeFillPercent)
    {
        if (_treesFilled)
            return;

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < Grid.GetLength(0); x++)
        {
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                if (Grid[x, y] == 0)
                {
                    Grid[x, y] = (pseudoRandom.Next(0, 100) < treeFillPercent) ? 2 : 0;
                }
            }
        }

        _treesFilled = true;
    }
}