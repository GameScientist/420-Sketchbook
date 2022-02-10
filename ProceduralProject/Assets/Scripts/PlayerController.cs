using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float gravity = -10f, xRot = 0, verticalVelocity;
    private Camera cam;
    private CharacterController pawn;
    private TextMeshProUGUI text;
    private int berries = 0;
    public int berryGoal = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
        pawn = GetComponent<CharacterController>();
        text = GameObject.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        text.text = "0/" + berryGoal;
    }

    // Update is called once per frame
    void Update()
    {
        pawn.Move(((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))) * 10f * Time.deltaTime);
        if (pawn.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetButton("Jump")) verticalVelocity = Mathf.Sqrt(80);
        }
        verticalVelocity += gravity * Time.deltaTime;
        pawn.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        xRot -= Input.GetAxis("Mouse Y");
        cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(xRot, -45f, 45f), 0, 0);
    }

    public void GameOver(bool victorius)
    {
        if (victorius) text.text = "You're stuffed!";
        else text.text = "You died!";
        Destroy(pawn);
        Destroy(this);
    }

    public void EatBerries()
    {
        berries++;
        if (berries >= berryGoal) GameOver(true);
        else text.text = berries + "/" + berryGoal;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (gameObject.GetComponent<Berry>() != null)
        {
            Destroy(collisionObject);
            berries++;
            if (berries >= berryGoal) GameOver(true);
            else text.text = berries + "/" + berryGoal;
        }
        else if (collisionObject.GetComponent<Cactus>() != null) GameOver(false);
    }
}
