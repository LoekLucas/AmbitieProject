using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisible : MonoBehaviour
{
    Renderer m_Renderer;

    public bool objectVisible;
    
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Renderer.isVisible)
        {
            objectVisible = true;
            //Debug.Log("Object is visible");
        }
        else
        {
            objectVisible = false;
        }
    }
}
