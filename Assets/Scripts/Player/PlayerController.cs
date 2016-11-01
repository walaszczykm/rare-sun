using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, rotateSpeed;

    private CharacterController controller;
    private Player player;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

	private void Update()
    {
        MovePlayer();
        RotateCameraAndPlayer();
        Shoot();
        SwitchWeapons();
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
        float mouseXRot = Input.GetAxis("Mouse X") * rotateSpeed;
        float mouseYRot = Input.GetAxis("Mouse Y") * rotateSpeed;

        Vector3 playerRotation = Vector3.up * mouseXRot;
        Vector3 cameraRotation = Vector3.right * mouseYRot;

        transform.Rotate(playerRotation);
        player.Cam.transform.Rotate(-cameraRotation);
    }

    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            player.Shoot();
        }
        if(Input.GetMouseButtonUp(0))
        {
            player.StopShooting();
        }
    }

    private void SwitchWeapons()
    {
        float mouseWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
        if(mouseWheelInput != 0.0f)
        {
            player.SwitchWeapon((int)Mathf.Sign(mouseWheelInput));
        }
    }
}