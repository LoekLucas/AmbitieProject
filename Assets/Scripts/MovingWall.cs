using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public GameObject player;
    public float distanceFromPlayer;
    private IsVisible movingWallVisible;

    // Start is called before the first frame update
    void Start()
    {
        movingWallVisible = GetComponent<IsVisible>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingWallVisible.objectVisible && distanceFromPlayer > 10f)
        {
            transform.position = new Vector3(player.transform.position.x + distanceFromPlayer, transform.position.y, transform.position.z);
            Debug.Log("Wall is moving!");
        }
        else
        {
            distanceFromPlayer = transform.position.x - player.transform.position.x;
        }
    }
}
