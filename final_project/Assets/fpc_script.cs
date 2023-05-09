using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CropNamespace;

namespace fpc_script_namespace{
public enum ItemType
{
    None,
    Shovel,
    Tomato
}
[RequireComponent(typeof(CharacterController))]
public class fpc_script : MonoBehaviour
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
    public GameObject tomatoPrefab;

    [SerializeField]
    public HUDManager hudmanager;

    [SerializeField]
    public GameObject shovelPrefab;

    [SerializeField]
    public Image crosshairImage; // Reference to the crosshair image component

    public RuntimeAnimatorController animatorController; // Reference to the animator controller

    public GameObject pigPrefab; // Reference to the pig prefab

    public float plantingRadius = 5f;

    private ItemType currentItem = ItemType.None;
    private GameObject currentItemObject = null; // variable to store the currently held item

    [SerializeField]
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;   
        crosshairImage.enabled = false;
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

    #region Handles Item Swapping


    if (Input.GetKeyDown(KeyCode.Alpha1) && currentItem != ItemType.Shovel)
    {
    // Destroy the previously held item
    if (currentItemObject != null) {
        Destroy(currentItemObject);
            currentItemObject = null; // add this line to set the value of currentItemObject to null after destroying the object
    }
        currentItem = ItemType.Shovel;
        // Instantiate the shovel prefab and position it in front of the camera
        GameObject newShovel = Instantiate(shovelPrefab, playerCamera.transform.position + playerCamera.transform.forward, Quaternion.identity);
        newShovel.transform.forward = playerCamera.transform.forward; // Set the shovel's forward direction to be the same as the camera's forward direction
        // Set the parent of the prefab to the camera so it stays in view
        newShovel.transform.SetParent(playerCamera.transform);
        // Set the local position of the prefab to the bottom right corner of the camera view
        newShovel.transform.localPosition = new Vector3(0.5f, -0.5f, 1f);
        newShovel.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f); // rotates the shovel by 30 degrees around the x-axis

        currentItemObject = newShovel; // update the currently held item
    }
    else if (Input.GetKeyDown(KeyCode.Alpha2) && currentItem != ItemType.Tomato)
    {
    // Destroy the previously held item
    if (currentItemObject != null) {
        Destroy(currentItemObject);
            currentItemObject = null; // add this line to set the value of currentItemObject to null after destroying the object
    }
        currentItem = ItemType.Tomato;
        // Instantiate the tomato prefab and position it in front of the camera
        GameObject newTomato = Instantiate(tomatoPrefab, playerCamera.transform.position + playerCamera.transform.forward, Quaternion.identity);
        newTomato.transform.forward = playerCamera.transform.forward; // Set the tomato's forward direction to be the same as the camera's forward direction
        newTomato.transform.SetParent(playerCamera.transform);
        newTomato.transform.localPosition = new Vector3(0.5f, -0.5f, 1f);

        currentItemObject = newTomato; // update the currently held item
    }

    #endregion


        #region Handles Planting and Harvesting and Feeding
            // Check if the player is aiming at a valid planting location
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // Check if the hit object is a valid planting surface
            if (hit.collider.gameObject.CompareTag("PlantingSurface") && Vector3.Distance(transform.position, hit.point) <= plantingRadius && currentItem == ItemType.Tomato)
            {
                // Show the crosshair
                // Note: you can use a separate GameObject to display the crosshair image, and enable/disable it here
                crosshairImage.enabled = true;

                // Check if the player presses the Plant key (e.g. Left Mouse Button)
                if (Input.GetMouseButtonDown(0))
                {
                    // Instantiate a new crop at the hit location
                    if (hudmanager.getTomatoes() > 0) {
                    GameObject newCrop = Instantiate(tomatoPrefab, hit.point, Quaternion.identity);
                    newCrop.AddComponent<CropScript>();
                    newCrop.tag = "TomatoPlant";
                    hudmanager.SubtractTomatoes(1);
                    }

                }
            } else if (hit.collider.gameObject.CompareTag("Pig") && Vector3.Distance(transform.position, hit.point) <= plantingRadius && currentItem == ItemType.Tomato)
            {
                // Show the crosshair
                // Note: you can use a separate GameObject to display the crosshair image, and enable/disable it here
                crosshairImage.enabled = true;

                // Check if the player presses the Plant key (e.g. Left Mouse Button)
                if (Input.GetMouseButtonDown(0))
                {
                    if (hudmanager.getTomatoes() > 0) {
                // Instantiate a new pig at the specified position
                GameObject newPig = Instantiate(pigPrefab, new Vector3(0f, 0f, 105.8f), Quaternion.identity);
                newPig.tag = "Pig";

                // Randomize the pig's rotation
                newPig.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                // Add an Animator component to the new pig
                Animator animator = newPig.AddComponent<Animator>();

                // Set the animator's controller to the assigned animator controller
                animator.runtimeAnimatorController = animatorController;

                // Add the animal_movement component to the new pig
                animal_movement animalMovement = newPig.AddComponent<animal_movement>();
                hudmanager.AddPigs(1);
                hudmanager.SubtractTomatoes(1);
                    }
                }
            } else if (hit.collider.gameObject.CompareTag("TomatoPlant") && Vector3.Distance(transform.position, hit.point) <= plantingRadius && currentItem == ItemType.Shovel) {
                                // Show the crosshair
                // Note: you can use a separate GameObject to display the crosshair image, and enable/disable it here
                crosshairImage.enabled = true;

                // Check if the player presses the Plant key (e.g. Left Mouse Button)
                if (Input.GetMouseButtonDown(0))
                {
                    // Instantiate a new crop at the hit location
                    hit.collider.gameObject.GetComponent<CropScript>().Harvest();
                    hudmanager.AddTomatoes(2);

                }
            }
            else
            {
                // Hide the crosshair
                crosshairImage.enabled = false;
            }
        }

        #endregion

    }
}
}