using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColumn : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    public float threshold;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 9; i++) if (Random.Range(0f, 1f) > threshold) Instantiate(wall, (i - 4) * Vector3.down + transform.position, transform.rotation, transform);
    }

    private void Update()
    {
        if (Camera.main.transform.position.x - transform.position.x > 9) Destroy(gameObject);
    }
}
