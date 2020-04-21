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
        if (GameController.isPlayerTurn() && !wasdPressed() && !cameraMoving)
        {
            move(Map.players[GameController.currState].getPlayer().transform.position);
        }
        else if (wasdPressed() && GameController.isPlayerTurn()) {
            if (Input.GetKeyDown(KeyCode.W)) {
                move(new Vector3( transform.position.x, transform.position.y + cameraSpeed, -10));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                move(new Vector3(transform.position.x - cameraSpeed, transform.position.y, -10));
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                move(new Vector3(transform.position.x, transform.position.y - cameraSpeed, -10));
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                move(new Vector3(transform.position.x + cameraSpeed, transform.position.y, -10));
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("TAB");
            cameraMoving = false;
        }
    }

    bool wasdPressed () {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)){
            cameraMoving = true;
            return true;
        }
        return false;
    }

    void move (Vector3 target) {
        Vector3 desiredPosition = target + cameraOffset;
        desiredPosition.z = -10f;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, cameraSmooth);
        smoothedPosition.z = -10f;
        transform.position = smoothedPosition;
    }
}
