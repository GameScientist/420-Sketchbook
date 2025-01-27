using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject wall;
    private AudioSource player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
        InvokeRepeating("GenerateColumn", 0, 0.015625f);
    }
    private void Update()
    {
        transform.position += Vector3.right * 64 * Time.deltaTime;
    }

    private void GenerateColumn()
    {
        int samples = 8;
        float[] data = new float[samples];
        player.GetOutputData(data, 0);
        for (int i = 0; i < data.Length; i++) Instantiate(wall, new Vector3(transform.position.x + 9 + Random.Range(-0.5f, 0.5f), i - 3.5f + Random.Range(-0.5f, 0.5f), 0), transform.rotation).transform.localScale *= data[i] + Random.Range(-0.1f, 0.1f);
    }
}
