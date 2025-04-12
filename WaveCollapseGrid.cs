using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollapseGrid : MonoBehaviour
{
    private int gridSizeX = 10;
    private int gridSizeY = 10;
    private WaveCollapseCell[,] waveFuncGrid;

    [SerializeField] bool animate = true;
    [SerializeField] float frameDelay = 1f;


    // Start is called before the first frame update
    void Start()
    {
        //Populate All Cells with Entropy
        waveFuncGrid = new WaveCollapseCell[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                WaveCollapseCell cell = new WaveCollapseCell(x, y);
                cell.initalize();
                waveFuncGrid[x, y] = cell;
            }
        }

        //Collapse Cells
        StartCoroutine(waveCollapse());

    }


    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator waveCollapse()
    {
        //Get the max amount of properties dynamically (WIP)
        int firstCellX = Random.Range(0, gridSizeX);
        int firstCellY = Random.Range(0, gridSizeY);
        int maxProps = waveFuncGrid[firstCellX, firstCellY].entropicProperties.Count;
        Debug.Log(maxProps);
        collapseCell(firstCellX, firstCellY);


        for (int currentMax = maxProps; currentMax > 0; currentMax--)
        {
            foreach (WaveCollapseCell parsedCell in waveFuncGrid)
            {
                if (parsedCell.entropicProperties.Count == currentMax && parsedCell.reduced)
                {
                    if (animate == true)
                    {
                        Debug.Log("Your mum");
                        yield return new WaitForSeconds(frameDelay);
                        collapseCell(parsedCell.x, parsedCell.y);
                    }
                }
                    
            }
        }

    }

    private void collapseCell(int collapsingCellX, int collapsingCellY)
    {
        WaveCollapseCell collapsingCell = waveFuncGrid[collapsingCellX, collapsingCellY];
        List<KeyValuePair<string, List<string>>> properties = collapsingCell.entropicProperties;

        //Entropic part of collapse
        KeyValuePair<string, List<string>> finalProperty = properties[Random.Range(0, properties.Count)];

        collapsingCell.entropicProperties.Clear();
        collapsingCell.entropicProperties.Add(finalProperty);
        Debug.Log(collapsingCell.entropicProperties[0].Key);
        collapsingCell.collapsed = true;
        reduceAdjacentCells(collapsingCellX, collapsingCellY);
    }

    private void reduceAdjacentCells(int collapsedCellX, int collapsedCellY)
    {
        WaveCollapseCell collapsedCell = waveFuncGrid[collapsedCellX, collapsedCellY];
        List<string> possibleAdjacentProps = collapsedCell.entropicProperties[0].Value;

        int north = collapsedCellY + 1;
        int south = collapsedCellY - 1;
        int east = collapsedCellX + 1;
        int west = collapsedCellX - 1;

        //int highestPropertyCount =
        reduceAdjacentCell(possibleAdjacentProps, collapsedCellX, north);
        reduceAdjacentCell(possibleAdjacentProps, collapsedCellX, south);
        reduceAdjacentCell(possibleAdjacentProps, east, collapsedCellY);
        reduceAdjacentCell(possibleAdjacentProps, west, collapsedCellY);
    }

    private void reduceAdjacentCell(List<string> possibleAdjProps, int adjCellX, int adjCellY)
    {
        //Nand equals (not 1 or not 2 I think)
        if (!(adjCellX < gridSizeX && adjCellX >= 0))
            return; //-1;
        if (!(adjCellY < gridSizeY && adjCellY >= 0))
            return; //-1;
        else
        {
            WaveCollapseCell adjCell = waveFuncGrid[adjCellX, adjCellY];
            if (adjCell.collapsed == false)
            {
                adjCell.entropicProperties = valuesPairsIntersection(possibleAdjProps, adjCell.entropicProperties);
                adjCell.reduced = true;
                //return adjCell.entropicProperties.Count;
            }
            //else
            //    return -2;
        }

    }

    private List<KeyValuePair<string, List<string>>> valuesPairsIntersection(List<string> values, List<KeyValuePair<string, List<string>>> pairs)
    {
        Debug.Log("New Cell");
        List<KeyValuePair<string, List<string>>> intersection = new();
        foreach (string value in values)
        {
            foreach (KeyValuePair<string, List<string>> pair in pairs) {
                if (pair.Key == value)
                {
                    Debug.Log("Value: " + value + " ," + "Adj Key: " + pair.Key);
                    intersection.Add(pair);
                }
            }
        }
        return intersection;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (waveFuncGrid[x, y] != null)
                {
                    if (waveFuncGrid[x, y].collapsed == true)
                    {
                        if (waveFuncGrid[x, y].entropicProperties[0].Key == "isGrass")
                            Gizmos.color = Color.green;
                        else if (waveFuncGrid[x, y].entropicProperties[0].Key == "isSand")
                            Gizmos.color = Color.yellow;
                        else if (waveFuncGrid[x, y].entropicProperties[0].Key == "isWater")
                            Gizmos.color = Color.blue;
                        Gizmos.DrawCube(new Vector3(x - 11, 0, y) * 5, new Vector3(5, 5, 5));
                    }

                    if (waveFuncGrid[x, y].entropicProperties.Count == 1)
                        Gizmos.color = Color.red;
                    else if (waveFuncGrid[x, y].entropicProperties.Count == 2)
                        Gizmos.color = Color.yellow;
                    else if (waveFuncGrid[x, y].entropicProperties.Count == 3)
                        Gizmos.color = Color.blue;
                    Gizmos.DrawCube(new Vector3(x, 0, y) * 5, new Vector3(5, 5, 5));
                }
            }
        }
    }

}
