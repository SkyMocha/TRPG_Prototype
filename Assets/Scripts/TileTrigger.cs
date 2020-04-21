using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //Debug.Log("TILE " + this.name + " CLICKED");
        if (isWalkable() && GameController.isPlayerTurn())
            // Gets the players array and changes the pos of the currState index
            Map.players[(int)GameController.currState].update.updatePosition (new Vector3 (this.transform.position.x, this.transform.position.y, -3));
    }

    // Checks tiletypes to see if that tile is able to be walked on
    private bool isWalkable () {
        foreach (int i in TileTypes.walkAble)
            if (i == id)
                return true;
        return false;
    }
}
