using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            int bitmask = 1 << 7;
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, ~bitmask)) print(hit.collider.gameObject);
            Debug.DrawRay(transform.position, transform.forward * 10);
        }
    }
}
