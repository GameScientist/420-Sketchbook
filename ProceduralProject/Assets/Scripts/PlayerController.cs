using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera cam;
    private CharacterController pawn;
    private float xRot = 0;
    private float verticalVelocity;
    private float gravity = -10f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
        pawn = GetComponent<CharacterController>();
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
}
