using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdate : MonoBehaviour
{

    public GameObject playerObject;
    public int id;
    public Vector3 curr_pos;
    public Player player;
    public int moveRadius;

    private void Start()
    {
        player.moveSphereObject.transform.localScale = new Vector3(5 * 3 / 5, 5 * 3 / 5);
        moveRadius = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updatePosition () {
        Debug.Log("MOVING PLAYER " + id);
        playerObject.transform.position = curr_pos;
        GameController.nextTurn();
    }

}
