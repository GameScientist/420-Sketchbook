using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("PoolLava", 0.25f);
    }

    private void SpreadLava()
    {
        if (EmptyNeighbor(-transform.up)) return;
        bool spreadable = false;
        foreach (Vector3 neighbor in new Vector3[] { transform.right, transform.forward, -transform.right, -transform.forward }) if(EmptyNeighbor(neighbor)) spreadable = true;
        if (!spreadable) EmptyNeighbor(transform.up);

    }

    private bool EmptyNeighbor(Vector3 neighbor)
    {
        bool emptyNeighbor = !Physics.Raycast(transform.position, neighbor, 1);
        if (emptyNeighbor) Instantiate(gameObject, transform.position + neighbor, transform.rotation);
        return emptyNeighbor;
    }
}
