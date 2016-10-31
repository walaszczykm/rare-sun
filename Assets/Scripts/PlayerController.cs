using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float runFactor = 2.0f;
    [SerializeField]
    private float rotateSpeed = 1.0f;
    [SerializeField]
    private Transform character;
    [SerializeField]
    private Transform cameraPoint;
    private CharacterController controller;

    void Awake()
    {
        controller = character.GetComponent<CharacterController>();
    }
	
	void FixedUpdate()
    {
        Jump();
        Move();
        Rotate();
        CameraUpdate();
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    private void Move()
    {
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) 
            ? runFactor : 1.0f);
        Vector3 input = character.right * Input.GetAxisRaw("Horizontal") + 
            character.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;
        controller.SimpleMove(input * speed);
    }

    private void Rotate()
    {
        Vector3 input = new Vector3(0, Input.GetAxis("Mouse X"), 0).normalized;
        character.Rotate(input * rotateSpeed);
    }

    private void CameraUpdate()
    {
        cameraPoint.rotation = Quaternion.Lerp(cameraPoint.rotation, character.rotation, Time.deltaTime * 10.0f);
        cameraPoint.position = Vector3.Lerp(cameraPoint.position, character.position, Time.deltaTime * 7.0f);
    }
}
