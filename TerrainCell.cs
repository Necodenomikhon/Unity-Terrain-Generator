using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCell : MonoBehaviour
{
    //Cell Properties
    public bool isWater = true;
    public bool isSand = true;
    public bool isGrass = true;

    //WaveFuncModel
    public bool collapsed = false;
    public int entropy = 3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
