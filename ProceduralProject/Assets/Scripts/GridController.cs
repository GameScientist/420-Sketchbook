using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public TerrainCube cubePrefab;

    // Start is called before the first frame update
    void Start()
    {
        MakeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeGrid()
    {
        int size = 19;
        for(int x = 0; x < size; x++)
        {
            for(int y = 0; y < size; y++)
            {
                Instantiate(cubePrefab, new Vector3(x, 0, y), Quaternion.identity);
            }
        }
    }
}
