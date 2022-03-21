using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCube : MonoBehaviour
{
    public Transform wall;
    BoxCollider box;
    bool isSolid;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider>();
        UpdateArt();
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        isSolid = !isSolid;
        UpdateArt();
    }

    void UpdateArt()
    {
        if (wall)
        {
            wall.gameObject.SetActive(isSolid);

            float y = isSolid ? .44f : 0;
            float h = isSolid ? 1.1f : .2f;

            box.size = new Vector3(1, 1, 1);
            box.center = new Vector3(0, y, 0);
        }
    }
}
