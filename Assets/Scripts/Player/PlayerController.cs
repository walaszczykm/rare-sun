using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Camera playerCam;
    [SerializeField]
    private float moveSpeed, rotateSpeed;

    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

	private void Update()
    {
        MovePlayer();
        RotateCameraAndPlayer();
	}

    private void MovePlayer()
    {
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? 2.0f : 1.0f);

        Vector3 input = transform.right * Input.GetAxisRaw("Horizontal") +
            transform.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;

        controller.SimpleMove(input * speed);  
    }

    private void RotateCameraAndPlayer()
    {
        Vector3 playerAngles = transform.up * Input.GetAxis("Mouse X") * rotateSpeed;

        transform.Rotate(playerAngles);
    }
}
