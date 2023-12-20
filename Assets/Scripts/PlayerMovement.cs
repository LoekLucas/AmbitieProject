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

    private Vector3 moveDirection;
    private Vector3 moveDirectionZ;
    private Vector3 moveDirectionX;
    private Vector3 velocity;

    private float gravityValue = -9.81f;
    private float jumpHeight = 3f;
    public float timesDoubleJumped = 0f;
    private float tpDistance = 1000f;
    private bool playerUp = false;
    private float playerLayer;

    private CharacterController characterController;

    // Start is called before the first frame update

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame

    void Update()
    {
        Move();;
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

        if (Input.GetMouseButtonDown(0))
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

    private void OnTriggerEnter(Collider collision)
    {
        string tag = collision.gameObject.tag;

        if (tag.StartsWith("Layer "))
        {
            string numericPart = tag.Substring("Layer ".Length);

            if (float.TryParse(numericPart, out playerLayer))
            {
                Debug.Log("Player Layer: " + playerLayer);
            }
            else
            {
                Debug.LogError("Failed to parse layer value from tag: " + tag);
            }
        }
        else
        {
            Debug.LogWarning("Tag does not start with 'Layer ': " + tag);
        }

        if (tag == "TeleportTrigger1")
        {

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
        transform.position = location;
        characterController.enabled = true;
    }
}
