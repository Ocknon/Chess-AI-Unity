using UnityEngine;
using System.Collections;

public class Overlays : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }
        }
    }
}
