using UnityEngine;

public class IsVisible : MonoBehaviour
{
    Renderer m_Renderer;
    public bool objectVisible;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }

    // Called when the object becomes visible to any camera
    void OnBecameVisible()
    {
        objectVisible = true;
    }

    // Called when the object is no longer visible to any camera
    void OnBecameInvisible()
    {
        objectVisible = false;
    }
}
