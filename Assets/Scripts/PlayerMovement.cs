using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml.Schema;

public class PlayerMovement : MonoBehaviour
{
    #region Variables and the like


    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] IsVisible IsVisible;
    [SerializeField] IsVisible IsVisible2;
    [SerializeField] IsVisible IsVisible3;
    [SerializeField] IsVisible IsVisible4;


    private Vector3 moveDirection;
    private Vector3 moveDirectionZ;
    private Vector3 moveDirectionX;
    private Vector3 velocity;

    private float gravityValue = -9.81f;
    private float jumpHeight = 3f;
    public float timesDoubleJumped = 0f;
    private float tpDistance;
    private bool playerUp = false;
    public float playerLayerNew;
    public float playerLayer = 1;
    private bool isColliding;
    private bool tpAllowed = true;
    public bool floorRemoveAllow = true;
    public float internalTimer;
    public float loops = 0;
    private bool allowLoopCount;
    private bool allowTimer = true;
    private bool hasTeleported = false;

    private CharacterController characterController;
    public GameObject removeableCube;
    public GameObject removeableFloor;

    #endregion

    #region Start and update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();

        tpDistance = playerLayer * 1000;
        isColliding = false;
    }
    #endregion

    #region Movement
    private void Move()
    {

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        // Setting Vector3's for horizontal and vertical movement
        moveDirectionZ = new Vector3(0, 0, moveZ);
        moveDirectionX = new Vector3(moveX, 0, 0);
        moveDirection = transform.TransformDirection(moveDirectionZ + moveDirectionX);

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }

        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        velocity.y += gravityValue * Time.deltaTime;
        
        if (Input.GetMouseButtonDown(1) && tpAllowed)
        {
            if (!playerUp)
            {
                TeleportPlayer(new Vector3(transform.position.x, transform.position.y + tpDistance, transform.position.z));
                playerUp = true;
            }

            else
            {
                TeleportPlayer(new Vector3(transform.position.x, transform.position.y - tpDistance, transform.position.z));
                playerUp = false;
            }
        }

        moveDirection += velocity;
        characterController.Move(moveDirection * Time.deltaTime);

    }
    #endregion

    #region Triggers
    private void OnTriggerStay(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.StartsWith("Layer "))
        {
            string numericPart = tag.Substring("Layer ".Length);

            if (float.TryParse(numericPart, out playerLayerNew))
            {
                if (playerLayer < playerLayerNew)
                {
                    playerLayer = playerLayerNew;
                }
            }
        }


        if (tag == "TeleportTrigger1" && IsVisible.objectVisible == false)
        {
            if (isColliding) return;
            isColliding = true;
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));

        }

        if (tag == "Layer 2" && IsVisible2.objectVisible == false)
        {
            GameObject.Destroy(removeableCube);
            GameObject.Destroy(removeableFloor, 5f);
            tpAllowed = false;
            if (internalTimer < 18 && allowTimer)
            {
                internalTimer += Time.deltaTime;
            }

            if (internalTimer >= 18)
            {
                TeleportPlayer(new Vector3(0, 3007.5f, 0));
                playerLayer = 1;
                tpAllowed = true;
                playerUp = false;
                playerLayer = 1;
                allowTimer = false;
                internalTimer = 0;
            }
        }

        if (tag == "TeleportTrigger2")
        {
            tpAllowed = false;
            if (isColliding) return;
            isColliding = true;
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "LoopCounter" && !hasTeleported)
        {
            loops++;

            if (loops >= 5)
            {
                TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));
                isColliding = false;
                hasTeleported = true; // Set the flag to true to prevent further teleportation
            }
        }
    }
    #endregion

    #region Functions
    private void Walk()
    {
        moveDirection *= walkSpeed;
    }

    private void Run()
    {
        moveDirection *= runSpeed;
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityValue);
    }

    private void Idle() { }

    private void TeleportPlayer(Vector3 location)
    {
        characterController.enabled = false;
        Quaternion savedRotation = transform.rotation;
        transform.SetPositionAndRotation(location, savedRotation);

        characterController.enabled = true;
    }
    #endregion
}
