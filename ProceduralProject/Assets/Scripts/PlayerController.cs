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
        cam = GetComponentInChildren<Camera>();
        pawn = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Footwork(Physics.Raycast(transform.position - (transform.up * 0.5f), Vector3.down, 0.1f));
        verticalVelocity += gravity * Time.deltaTime;
        pawn.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        InterpretCameraInput(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).normalized);
        cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(xRot, -45f, 45f), 0, 0);
    }

    private void Footwork(bool grounded)
    {
        if (grounded) verticalVelocity = -1f;
        pawn.Move(((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))) * 10f * Time.deltaTime);
        if (grounded && Input.GetButton("Jump")) verticalVelocity = -Mathf.Sqrt(1) * gravity;
    }

    /// <summary>
    /// Rotates the player and camera based on collected input.
    /// </summary>
    /// <param name="lookInput">The input given by the player ordering the character change the character and camera rotation.</param>
    private void InterpretCameraInput(Vector2 lookInput)
    {
        transform.Rotate(transform.up * lookInput.x * Time.deltaTime * 720f);
        xRot -= lookInput.y * 360f * Time.deltaTime;
    }

}
