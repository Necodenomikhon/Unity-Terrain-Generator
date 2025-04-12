using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private int mapSize;
    
    //Layer 1 piece generation
    [SerializeField] private GameObject terrainLayer1;
    [SerializeField] private int layer1Height;
    [SerializeField] [Range(0f, 10f)] private int maxLayer1Size;
    [SerializeField] [Range(0f, 50f)] private int layer1Frequency;

    //Layer 2 piece generation
    [SerializeField] private GameObject terrainLayer2;
    [SerializeField] private int layer2Height;
    [SerializeField] [Range(0f, 10f)] private int maxLayer2Size;
    [SerializeField] [Range(0f, 25f)] private int layer2Frequency;

    void Start()
    { 
        generateLayer1();
        //generateLayer2();
    }
    private void generateLayer1()
    {
        for (int i = 0; i < layer1Frequency; i++)
        {
            GameObject layer1Piece = Instantiate(terrainLayer1, new Vector3(Random.Range(0f, 1f) * mapSize, 0, Random.Range(0f, 1f) * mapSize), new Quaternion(0, 0, 0, 0), gameObject.transform);
            layer1Piece.transform.localScale = new Vector3(Mathf.Ceil(Random.Range(0f, maxLayer1Size)), layer1Height, Mathf.Ceil(Random.Range(0f, maxLayer1Size)));
            layer1Piece.gameObject.tag = "TerrainLayer1";
        }   
    }
    private void generateLayer2()
    {
        if (GameObject.Find("Environment/Terrain/GrassLayer(Clone)") != null)
        {
            for (int i = 0; i < layer2Frequency; i++)
            {
                GameObject layer2Piece = Instantiate(terrainLayer1, new Vector3(Random.Range(0f, 1f) * mapSize, layer1Height, Random.Range(0f, 1f) * mapSize), new Quaternion(0, 0, 0, 0), gameObject.transform);
                layer2Piece.transform.localScale = new Vector3(Mathf.Ceil(Random.Range(0f, maxLayer2Size)), layer2Height, Mathf.Ceil(Random.Range(0f, maxLayer2Size)));
                //Method of Layer 2 Generation: keep or destroy blocks that are flush above layer pieces.
                bool keep = false;

                //Parses raycasts from layer2Piece vertices to detect layer1Pieces
                Vector3[] layer2PieceVertices = layer2Piece.GetComponent<MeshFilter>().mesh.vertices;
                HashSet<Vector3> uniqueVertices = new HashSet<Vector3>(layer2PieceVertices);
                List<bool> vertIsFlush = new List<bool>();

                foreach (Vector3 vertex in uniqueVertices)
                {
                    Vector3 vertexWorldPosition = layer2Piece.transform.TransformPoint(vertex);
                    RaycastHit hit;
                    Debug.DrawRay(vertexWorldPosition, -layer2Piece.transform.up, Color.red, Mathf.Infinity);
                    if (Physics.Raycast(vertexWorldPosition, -layer2Piece.transform.up, out hit))
                    {
                        if (hit.collider.gameObject.tag == "TerrainLayer1")
                        {
                            keep = true;
                            vertIsFlush.Add(true);
                            //Constrains layer2Piece sizes to below layer1Piece sizes
                            GameObject tL1 = hit.collider.gameObject;
                            if (layer2Piece.transform.localScale.x > tL1.transform.localScale.x)
                            {
                                layer2Piece.transform.localScale = new Vector3(tL1.transform.localScale.x, layer2Height, layer2Piece.transform.localScale.y);
                            }
                            if (layer2Piece.transform.localScale.y > tL1.transform.localScale.y)
                            {
                                layer2Piece.transform.localScale = new Vector3(layer2Piece.transform.localScale.x, layer2Height, tL1.transform.localScale.y);
                            }
                        }
                        else
                        {
                            vertIsFlush.Add(false);
                        }
                    }
                }
                if (keep)
                {
                    for (int j = 0; j < vertIsFlush.Count; j++)
                    {
                        if (vertIsFlush[j] == false)
                        {
                            //Move layer2Piece
                            Destroy(layer2Piece);
                            i--;
                        }
                    }
                }
                else
                {
                    Destroy(layer2Piece);
                    i--;
                }
            }
        }
    }



    public void EditorGenerateLayer1()
    {
        for (int i = 0; i < layer1Frequency; i++)
        {
            GameObject layer1Piece = Instantiate(terrainLayer1, new Vector3(Random.Range(0f, 1f) * mapSize, 0, Random.Range(0f, 1f) * mapSize), new Quaternion(0, 0, 0, 0), gameObject.transform);
            layer1Piece.transform.localScale = new Vector3(Mathf.Ceil(Random.Range(0f, maxLayer1Size)), layer1Height, Mathf.Ceil(Random.Range(0f, maxLayer1Size)));
            layer1Piece.gameObject.tag = "TerrainLayer1";
        }
    }
    public void EditorGenerateLayer2()
    {
        if (GameObject.Find("Environment/Terrain/GrassLayer(Clone)") != null)
        {
            for (int i = 0; i < layer2Frequency; i++)
            {
                GameObject layer2Piece = Instantiate(terrainLayer1, new Vector3(Random.Range(0f, 1f) * mapSize, layer1Height, Random.Range(0f, 1f) * mapSize), new Quaternion(0, 0, 0, 0), gameObject.transform);
                layer2Piece.transform.localScale = new Vector3(Mathf.Ceil(Random.Range(0f, maxLayer2Size)), layer2Height, Mathf.Ceil(Random.Range(0f, maxLayer2Size)));
                //Method of Layer 2 Generation: keep blocks that are flush above layer pieces.
                bool keep = false;

                //Eventually Detects whether layer2pieces are flush above layer1pieces
                Vector3[] layer2PieceVertices = layer2Piece.GetComponent<MeshFilter>().sharedMesh.vertices;
                HashSet<Vector3> uniqueVertices = new HashSet<Vector3>(layer2PieceVertices);

                foreach (Vector3 vertex in uniqueVertices)
                {
                    Vector3 vertexWorldPosition = layer2Piece.transform.TransformPoint(vertex);
                    RaycastHit hit;
                    Debug.DrawRay(vertexWorldPosition, -layer2Piece.transform.up, Color.red, Mathf.Infinity);

                    if (Physics.Raycast(vertexWorldPosition, -layer2Piece.transform.up, out hit))
                    {
                        if (hit.collider.gameObject.tag == "TerrainLayer1")
                        {
                            Debug.Log(hit.collider.gameObject.name);
                            keep = true;
                        }
                    }
                }
                if (keep == false)
                {
                    DestroyImmediate(layer2Piece);
                    i--;
                }
            }
        }
    }

    public int getLastHeight()
    {
        return layer2Height;
    }
    //Another generation method: game of life with spherecasts
    //Another generation method: gridded layer generation (supports terrain wall collision)
    //Another generation method: resized noise texture
}
