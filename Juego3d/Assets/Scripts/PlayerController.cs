using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float rotatioSpeed;
    [SerializeField] float gravityScale;
    private Vector3 moveDirection;

    CharacterController playerController;
    public Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
        }
        float yStore = moveDirection.y;

        moveDirection = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
        moveDirection = moveDirection * moveSpeed;

        moveDirection.y = yStore;

        if (Input.GetKey(KeyCode.Space))
        {
            moveDirection.y = jumpForce;
        }

        moveDirection.y += Physics.gravity.y * Time.deltaTime * gravityScale;

        Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotatioSpeed * Time.deltaTime);

        playerController.Move(moveDirection * Time.deltaTime);
    }
}
