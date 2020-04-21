using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{

    // When the sphere is clicked, clicked is equal to true and then back to false when the mouse is pressed up
    public bool clicked = false;

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) {
            clicked = false;
        }
    }

    void OnMouseDown()
    {
        clicked = true;
    }
}
