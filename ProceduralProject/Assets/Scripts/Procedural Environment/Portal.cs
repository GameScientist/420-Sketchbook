using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = Vector3.up * 52.5f;
        other.GetComponent<PlayerController>().panel.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
