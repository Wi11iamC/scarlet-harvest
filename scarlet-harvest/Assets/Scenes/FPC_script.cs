using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPC_script_namespace{
[RequireComponent(typeof(CharacterController))]
public class FPC_script : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 45f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    [SerializeField]
    
    CharacterController characterController;

    [SerializeField]
    public GameObject treePrefab;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
    }

    // Update is called once per frame
    void Update()
    {
        #region Handles Movment
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        #endregion

        #region Handles Jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        #endregion


        #region Handles Rotation
        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        #endregion

// Add tree to the terrain when the left mouse button is pressed
    if (Input.GetMouseButtonDown(0))
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, LayerMask.GetMask("Terrain")))
        {
            // Get the height of the terrain at the hit point
            float terrainHeight = Terrain.activeTerrain.SampleHeight(hit.point);

            // Add a small offset to raise the tree above the terrain
            Vector3 treePosition = new Vector3(hit.point.x, terrainHeight + 0.05f, hit.point.z);

            // Instantiate the tree prefab at the adjusted position
            Instantiate(treePrefab, treePosition, Quaternion.identity);
        }
    }
    }
}
}