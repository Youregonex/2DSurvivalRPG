using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int dimensions;

    public WFCTile[] tileObjects;
    public List<WFCCell> gridComponents;
    public WFCCell cellObject;

    private int iterations = 0;


    private void Awake()
    {
        gridComponents = new();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        for(int x = 0; x < dimensions; x++)
        {
            for(int y = 0; y < dimensions; y++)
            {
                WFCCell newCell = Instantiate(cellObject, new Vector2(x, y), Quaternion.identity);
                newCell.CreateCell(false, tileObjects);
                gridComponents.Add(newCell);
            }
        }

        StartCoroutine(CheckEntropy());
    }

    private IEnumerator CheckEntropy()
    {
        List<WFCCell> tempGrid = new(gridComponents);

        tempGrid.RemoveAll(c => c.isCollapsed);
        tempGrid.Sort((a, b) => { return a.tileOptions.Length - b.tileOptions.Length; });

        int arrLength = tempGrid[0].tileOptions.Length;
        int stopIndex = default;

        for(int i = 1; i < tempGrid.Count; i++)
        {
            if(tempGrid[i].tileOptions.Length > arrLength)
            {
                stopIndex = i;
                break;
            }
        }

        if(stopIndex > 0)
        {
            tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);
        }

        yield return new WaitForSeconds(.01f);

        CollapseCell(tempGrid);
    }

    private void CollapseCell(List<WFCCell> tempGrid)
    {
        int randomIndex = UnityEngine.Random.Range(0, tempGrid.Count);

        WFCCell cellToCollapse = tempGrid[randomIndex];

        cellToCollapse.isCollapsed = true;
        WFCTile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
        cellToCollapse.tileOptions = new WFCTile[] { selectedTile };

        WFCTile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, Quaternion.identity);

        UpdateGeneration();
    }

    private void UpdateGeneration()
    {
        List<WFCCell> newGenerationCell = new(gridComponents);

        for(int y = 0; y < dimensions; y++)
        {
            for(int x = 0; x < dimensions; x++)
            {
                var index = x + y * dimensions;

                if(gridComponents[index].isCollapsed)
                {
                    newGenerationCell[index] = gridComponents[index];
                }
                else
                {
                    List<WFCTile> options = new();

                    foreach (WFCTile tile in tileObjects)
                    {
                        options.Add(tile);
                    }

                    if(y > 0)
                    {
                        WFCCell up = gridComponents[x + (y - 1) * dimensions].GetComponent<WFCCell>();
                        List<WFCTile> validOptions = new();

                        foreach (WFCTile possibleOption in up.tileOptions)
                        {
                            var valOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                            var valid = tileObjects[valOption].topNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (x < dimensions - 1)
                    {
                        WFCCell right = gridComponents[x + 1 + y * dimensions].GetComponent<WFCCell>();
                        List<WFCTile> validOptions = new List<WFCTile>();

                        foreach (WFCTile possibleOption in right.tileOptions)
                        {
                            var valOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                            var valid = tileObjects[valOption].leftNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (y < dimensions - 1)
                    {
                        WFCCell bot = gridComponents[x + (y + 1) * dimensions].GetComponent<WFCCell>();
                        List<WFCTile> validOptions = new List<WFCTile>();

                        foreach (WFCTile possibleOption in bot.tileOptions)
                        {
                            var valOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                            var valid = tileObjects[valOption].bottomNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    if (x > 0)
                    {
                        WFCCell left = gridComponents[(x - 1) + y * dimensions].GetComponent<WFCCell>();
                        List<WFCTile> validOptions = new List<WFCTile>();

                        foreach (WFCTile possibleOption in left.tileOptions)
                        {
                            var valOption = Array.FindIndex(tileObjects, obj => obj == possibleOption);
                            var valid = tileObjects[valOption].rightNeighbours;

                            validOptions = validOptions.Concat(valid).ToList();
                        }

                        CheckValidity(options, validOptions);
                    }

                    WFCTile[] newTileList = new WFCTile[options.Count];

                    for(int i = 0; i < options.Count; i++)
                    {
                        newTileList[i] = options[i];
                    }

                    newGenerationCell[index].RecreateCell(newTileList);
                }
            }
        }

        gridComponents = newGenerationCell;
        iterations++;

        if(iterations < dimensions * dimensions)
        {
            StartCoroutine(CheckEntropy());
        }
    }

    private void CheckValidity(List<WFCTile> optionList, List<WFCTile> validOption)
    {
        for(int x = optionList.Count - 1; x >= 0; x--)
        {
            var element = optionList[x];

            if(!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }


}
