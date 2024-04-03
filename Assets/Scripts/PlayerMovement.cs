using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Xml.Schema;
using Unity.VisualScripting;

public class PlayerMovement : MonoBehaviour
{
    #region Variables and the like


    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] IsVisible IsVisible;
    [SerializeField] IsVisible IsVisible2;
    [SerializeField] IsVisible IsVisible3;
    [SerializeField] IsVisible IsVisible4;
    [SerializeField] IsVisible IsVisible5;
    [SerializeField] IsVisible IsVisible6;
    [SerializeField] IsVisible IsVisible7;
    [SerializeField] IsVisible IsVisible8;
    [SerializeField] IsVisible IsVisible9;
    [SerializeField] IsVisible IsVisible10;
    [SerializeField] IsVisible IsVisible11;
    [SerializeField] IsVisible IsVisible12;
    [SerializeField] IsVisible IsVisible13;
    [SerializeField] IsVisible IsVisible14;
    [SerializeField] IsVisible IsVisible15;
    [SerializeField] IsVisible IsVisible16;
    [SerializeField] IsVisible IsVisible17;
    [SerializeField] IsVisible IsVisible18;
    [SerializeField] IsVisible isVisible19;
    [SerializeField] IsVisible isVisible20;

    public GameObject removeableCube;
    public GameObject removeableFloor;
    public GameObject removeableCube2;
    public GameObject removeableCube3;
    public GameObject removeableCube4;
    public GameObject removeableCube5;
    public GameObject removeableCube6;
    public GameObject removeableCube7;
    public GameObject removeableCube8;
    public GameObject removeableCube9;
    public GameObject removeableCube10;
    public GameObject removeableCube11;
    public GameObject removeableCube12;
    public GameObject removeableCube13;
    public GameObject removeableCube14;
    public GameObject removeableCube15;
    public GameObject removeableCube16;




    private Vector3 moveDirection;
    private Vector3 moveDirectionZ;
    private Vector3 moveDirectionX;
    private Vector3 velocity;

    private float gravityValue = -9.81f;
    private float tpDistance;
    public bool playerUp = false;
    public float playerLayerNew;
    public float playerLayer = 1;
    private bool isColliding;
    private bool tpAllowed = true;
    public bool floorRemoveAllow = true;
    public float internalTimer;
    public float loops = 0;
    private bool allowTimer = true;
    private bool hasTeleported = false;
    private float TeleportDelay = 0;
    private bool choseBrightLayer;
    public bool hasKey = false;

    public bool isVisible4Exists = true;
    public bool isVisible5Exists = true;
    public bool isVisible6Exists = true;
    public bool isVisible7Exists = true;
    public bool isVisible8Exists = true;

    private CharacterController characterController;

    #endregion

    #region Start and update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();

        if (choseBrightLayer)
        {
            tpDistance = playerLayer * -1000;
        }

        else
        { 
            tpDistance = playerLayer * 1000;
        }
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

        if (tag == "First TP Check")
        {
            GameObject.Destroy(removeableCube12);
        }

        #region Dark Layer choice
        if (tag == "Dark Choice" && IsVisible.objectVisible == false)
        {
            if (isColliding) return;
            isColliding = true;
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));

        }

        #region Big white cube fall
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

            #region Transition to gray
            if (tag == "Layer 3")
            {
                tpAllowed = false;

                if (IsVisible4.objectVisible == false && isVisible4Exists)
                {
                    isVisible4Exists = false;
                    GameObject.Destroy(removeableCube2);
                    TeleportDelay++;
                }

                if (IsVisible5.objectVisible == false && isVisible5Exists)
                {
                    isVisible5Exists = false;
                    GameObject.Destroy(removeableCube3);
                    TeleportDelay++;
                }

                if (IsVisible6.objectVisible == false && isVisible6Exists)
                {
                    isVisible6Exists = false;
                    GameObject.Destroy(removeableCube4);
                    TeleportDelay++;
                }

                if (IsVisible7.objectVisible == false && isVisible7Exists)
                {
                    isVisible7Exists = false;
                    GameObject.Destroy(removeableCube5);
                    TeleportDelay++;
                }

                if (IsVisible8.objectVisible == false && isVisible8Exists || IsVisible5.objectVisible && isVisible8Exists)
                {
                    isVisible8Exists = false;
                    GameObject.Destroy(removeableCube6);
                    TeleportDelay++;
                }


                if (IsVisible3.objectVisible == false && TeleportDelay >= 5)
                {
                    if (isColliding) return;
                    isColliding = true;
                    TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));
                }
            }
            #endregion

        }
        #endregion

        #region Transition to gray
        if (tag == "Layer 3")
        {
            tpAllowed = false;

            if (IsVisible4.objectVisible == false && isVisible4Exists)
            {
                isVisible4Exists = false;
                GameObject.Destroy(removeableCube2);
                TeleportDelay++;
            }

            if (IsVisible5.objectVisible == false && isVisible5Exists)
            {
                isVisible5Exists = false;
                GameObject.Destroy(removeableCube3);
                TeleportDelay++;
            }

            if (IsVisible6.objectVisible == false && isVisible6Exists)
            {
                isVisible6Exists = false;
                GameObject.Destroy(removeableCube4);
                TeleportDelay++;
            }

            if (IsVisible7.objectVisible == false && isVisible7Exists)
            {
                isVisible7Exists = false;
                GameObject.Destroy(removeableCube5);
                TeleportDelay++;
            }

            if (IsVisible8.objectVisible == false && isVisible8Exists || IsVisible5.objectVisible && isVisible8Exists)
            {
                isVisible8Exists = false;
                GameObject.Destroy(removeableCube6);
                TeleportDelay++;
            }


            if (IsVisible3.objectVisible == false && TeleportDelay >= 5)
            {
                if (isColliding) return;
                isColliding = true;
                TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));
            }
        }
        #endregion

        #region Removing walls on last Layer
        if (tag == "Last Layer")
        {
            internalTimer += Time.deltaTime;
            if (internalTimer >= 10)
            {
                if (IsVisible9.objectVisible == false)
                {
                    GameObject.Destroy(removeableCube7);
                }

                if (IsVisible10.objectVisible == false)
                {
                    GameObject.Destroy(removeableCube8);
                }

                if (IsVisible11.objectVisible == false)
                {
                    GameObject.Destroy(removeableCube9);
                }

                if (IsVisible12.objectVisible == false)
                {
                    GameObject.Destroy(removeableCube10);
                }

                if (IsVisible13.objectVisible == false)
                {
                    GameObject.Destroy(removeableCube11);
                }
            }
        }
        #endregion

        #region TP to loop
        if (tag == "TeleportTrigger2")
        {
            tpAllowed = false;
            if (isColliding) return;
            isColliding = true;
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y + 1000, transform.position.z));
        }
        #endregion

        #endregion

        #region Bright layer choice
        if (tag == "Bright Choice" && IsVisible14.objectVisible == false)
        {
            choseBrightLayer = true;
            if (isColliding) return;
            isColliding = true;
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y - 1000, transform.position.z));
        }

        #region Key
        if (tag == "Key")
        {
            hasKey = true;
            GameObject.Destroy(removeableCube14);
        }
        #endregion

        #region Moving Corridor Logic
        if (tag == "movingCorridor")
        {
            tpAllowed = false;
            if (hasKey)
            {
                TeleportPlayer(new Vector3(84.7944031f, -2998.42017f, -1.64473557f));
            }
        }

        //if (!IsVisible15)
        //{
        //    Debug.Log("!IsVisible15");
        //    if (IsVisible16)
        //    {
        //        Debug.Log("IsVisible16");
        //        if (hasKey)
        //        {
        //            Debug.Log("hasKey");
        //        }
        //    }
        //}
        #endregion  

        #region Big Door Logic
        if (tag == "Big Door")
        {
            TeleportPlayer(new Vector3(-9.31221962f, -3426.68042f, -0.927093744f));
        }
        #endregion

        #endregion

    }

    private void OnTriggerEnter(Collider collision)
    {
        string tag = collision.gameObject.tag;

        #region Loop Counter
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
        #endregion
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