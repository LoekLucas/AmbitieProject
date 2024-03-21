using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMovingWall : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
         transform.position = new Vector3(player.transform.position.x - 40f, transform.position.y, transform.position.z);
    }
}
