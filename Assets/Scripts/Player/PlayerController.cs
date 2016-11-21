using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(Player))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed, rotateSpeed, dashingTime;

    private CharacterController controller;
    private Player player;
    private bool isDashing;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
    }

	private void Update()
    {
        TestIfNeedToDash();
        MovePlayer();
        RotateCameraAndPlayer();
        Shoot();
        SwitchWeapons();
	}

    private void MovePlayer()
    {
        float speed = moveSpeed * (isDashing ? 2.5f : 1.0f);

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
            player.SwitchWeaponWithDirection((int)Mathf.Sign(mouseWheelInput));
        }
        else
        {
            for(int i=0; i<9; i++)
            {
                KeyCode code = (KeyCode)(49 + i);
                if(Input.GetKeyDown(code))
                {
                    player.SwitchWeapon(i);
                }
            }
        }
    }

    private void TestIfNeedToDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
    }
}