using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float cameraSmooth = 0.125f;
    public Vector3 cameraOffset;
    public GameObject player;

    float cameraSpeed = 0.5f;

    bool cameraMoving;

    private void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Moves the camera towards the current player
        if (GameController.isPlayerTurn() && !wasdPressed() && !cameraMoving)
        {
            move(Map.players[GameController.currState].getPlayer().transform.position);
        }
        // Moves the camera with WASD
        else if (wasdPressed() && GameController.isPlayerTurn()) {
            if (Input.GetKey(KeyCode.W)) {
                move(new Vector3( transform.position.x, transform.position.y + cameraSpeed, -10));
            }
            if (Input.GetKey(KeyCode.A))
            {
                move(new Vector3(transform.position.x - cameraSpeed, transform.position.y, -10));
            }
            if (Input.GetKey(KeyCode.S))
            {
                move(new Vector3(transform.position.x, transform.position.y - cameraSpeed, -10));
            }
            if (Input.GetKey(KeyCode.D))
            {
                move(new Vector3(transform.position.x + cameraSpeed, transform.position.y, -10));
            }
        }
        // Moves the camera back to the player
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraMoving = false;
        }
    }

    // Checks to see if WASD is being pressed
    bool wasdPressed () {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)){
            cameraMoving = true;
            return true;
        }
        return false;
    }

    // Moves the camera to a specific Vector3
    void move (Vector3 target) {
        Vector3 desiredPosition = target + cameraOffset;
        desiredPosition.z = -10f;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSmooth);
        smoothedPosition.z = -10f;
        transform.position = smoothedPosition;
    }
}
