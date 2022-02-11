using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float xRot = 0, verticalVelocity;
    private Camera cam;
    private CharacterController pawn;
    private int berries = 0;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private GameObject gameOver;
    public GameObject panel;

    public static PlayerController singleton { get; private set; }

    private void Awake()
    {
        if (singleton)
        {
            print(singleton);
            score.text = singleton.berries.ToString();
            Destroy(gameObject);
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        transform.position = Vector3.up * 52.5f;
        score.text = berries.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponentInChildren<Camera>();
        pawn = GetComponent<CharacterController>();
        panel = GetComponentInChildren<Image>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        pawn.Move(((transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"))) * 5f * Time.deltaTime);
        if (pawn.isGrounded)
        {
            verticalVelocity = -1f;
            if (Input.GetButton("Jump")) verticalVelocity = Mathf.Sqrt(80);
        }
        verticalVelocity -= 10 * Time.deltaTime;
        pawn.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
        xRot -= Input.GetAxis("Mouse Y");
        cam.transform.localRotation = Quaternion.Euler(Mathf.Clamp(xRot, -90f, 90f), 0, 0);
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        Destroy(pawn);
        Destroy(this);
    }

    public void EatBerries()
    {
        berries++;
        score.text = berries.ToString();
    }
}
