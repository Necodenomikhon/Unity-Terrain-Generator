using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollapseCell
{
    public WaveCollapseCell(int xPos, int yPos)
    {
        x = xPos;
        y = yPos;
    }
    //Grid Position
    public int x;
    public int y;

    //Properties
    const string WATER = "isWater";
    const string GRASS = "isGrass";
    const string SAND = "isSand";

    //Adjacency Rules
    public readonly List<string> WATERADJS = new() { WATER, SAND };
    public readonly List<string> SANDADJS = new() { SAND, WATER, GRASS };
    public readonly List<string> GRASSADJS = new() { GRASS, SAND };

    //Property Properties
    KeyValuePair<string, List<string>> waterProperty;
    KeyValuePair<string, List<string>> sandProperty;
    KeyValuePair<string, List<string>> grassProperty;

    public List<KeyValuePair<string, List<string>>> entropicProperties = new();
    public bool collapsed = false;
    public bool reduced = false;

    // Start is called before the first frame update
    public void initalize()
    {
        //Initialize property properties
        waterProperty = new(WATER,  WATERADJS);
        sandProperty = new(SAND, SANDADJS);
        grassProperty = new(GRASS, GRASSADJS);

        Debug.Log("Running");
        entropicProperties.Add(waterProperty);
        entropicProperties.Add(sandProperty);
        entropicProperties.Add(grassProperty);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
