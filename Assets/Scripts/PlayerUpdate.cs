﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpdate : MonoBehaviour
{

    public GameObject playerObject;
    public int id;
    public Vector3 curr_pos;
    public Player player;
    public int moveRadius;
    public int initiative;
    public int order;
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        player.moveSphereObject.transform.localScale = new Vector3(5 * 3 / 5, 5 * 3 / 5);
        moveRadius = 5;
    }

    // Update is called once per frame
    void Update()
    {
        actionHandler();
    }

    public void updatePosition (Vector3 pos) {
        Debug.Log("MOVING PLAYER " + id);

        if (GameController.isPlayerTurn(id) && Map.inCircle(curr_pos, pos, moveRadius))
        {
            curr_pos = pos;
            playerObject.transform.position = curr_pos;

            Map.updateFogOfWar();
            GameController.nextTurn();
        }
    }

    public bool isActionable () {
        return GameController.isPlayerTurn(id) || GameController.isShooting();
    }

    public void actionHandler () {
        if (isActionable())
        {
            // Does the player want to shoot?
            if (Input.GetKeyDown(KeyCode.E) && !GameController.isShooting())
            {
                Map.drawRadius(curr_pos);
                GameController.setShooting();
                Debug.Log("SHOOT");
            }
            // Does the player want to cancel a shot?
            else if (Input.GetKeyDown(KeyCode.Escape) && GameController.isShooting())
            {
                Map.updateFogOfWar();
                GameController.cancelShooting();
                Debug.Log("CANCEL SHOOT");
            }

        }
    }

    public int CompareTo(Player two)
    {
        if (initiative > two.initiative)
            return 1;
        else if (initiative == two.initiative)
            return 0;
        else
            return -1;
    }

}