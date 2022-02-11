using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public int iterations;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SpreadLava", 0.25f);
    }

    private void SpreadLava()
    {
        if (EmptyNeighbor(-transform.up) || iterations == 40) return;
        bool spreadable = false;
        foreach (Vector3 neighbor in new Vector3[] { transform.right, transform.forward, -transform.right, -transform.forward }) if(EmptyNeighbor(neighbor)) spreadable = true;
        if (!spreadable) EmptyNeighbor(transform.up);

    }

    private bool EmptyNeighbor(Vector3 neighbor)
    {
        bool emptyNeighbor = !Physics.Raycast(transform.position, neighbor, 1);
        if (emptyNeighbor) Instantiate(gameObject, transform.position + neighbor, transform.rotation).GetComponent<Lava>().iterations = iterations + 1;
        return emptyNeighbor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player")) collision.transform.GetComponent<PlayerController>().GameOver();
        Destroy(this);
    }
}
