using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    CharacterController controller;
    public Vector3 playerVelocity;
    public bool groundedPlayer;
    float playerSpeed = 5.0f;
    float jumpHeight = 1.0f;
    float gravityValue = -9.81f;
    public Transform shoot;
    public GameObject cube;

    RaycastHit hit;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = shoot.right * Input.GetAxis("Horizontal") + shoot.forward * Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            DestroyObject();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            SpawnObject();

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void DestroyObject()
	{
        if (Physics.Raycast(shoot.position, shoot.forward, out hit, 150f))
            Destroy(hit.transform.gameObject);
    }

    void SpawnObject()
	{
        if (Physics.Raycast(shoot.position, shoot.forward, out hit, 150f))
        {
            Debug.DrawRay(shoot.position, shoot.forward * hit.distance, Color.black, 5f);
            Debug.DrawRay(hit.point, hit.normal, Color.magenta, 5f);
            Vector3 pos = Vector3Int.FloorToInt(hit.point + Vector3.one * .5f + (hit.normal * .5f));
            Instantiate(cube, pos, Quaternion.identity);
        }
    }
}
