using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml.Schema;

public class PlayerMovement : MonoBehaviour
{
    // Variables and the like
    

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] IsVisible IsVisible;
    [SerializeField] IsVisible IsVisible2;


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

    private CharacterController characterController;
    public GameObject removeableCube;
    public GameObject removeableFloor;

    // Start is called before the first frame update

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame

    void Update()
    {
        Move();

        tpDistance = playerLayer * 1000;
        isColliding = false;
    }

    private void Move()
    {

        if (characterController.isGrounded && velocity.y < 0) //?
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

        if (timesDoubleJumped <= 1f && (!Input.GetKey(KeyCode.Mouse4)))
        {

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (moveDirection != Vector3.zero)
            {
                Idle();
            }
        }

        if (characterController.isGrounded)
        {
            timesDoubleJumped = 0;
        }

        else
        {
            velocity.y += gravityValue * Time.deltaTime;
        }

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
            // Add a 5 sec delay here
            TeleportPlayer(new Vector3(0, 7.5f, 0));
            playerLayer = 1;
            tpAllowed = true;
        }
    }

    // Make it so playerLayer only changes when new layer is higher than old
    // This would allow for me to make a formula that dictates teleport distance dynamically


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
}
