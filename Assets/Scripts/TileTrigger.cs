using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTrigger : MonoBehaviour
{
    public int id;
    public Vector3 curr_pos;
    List<Node> currPath;

    // Start is called before the first frame update
    void Start()
    {
        curr_pos = new Vector3(this.transform.position.x, this.transform.position.y, -3);
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

    private void OnMouseOver()
    {
        if (isWalkable() && 
            GameController.isPlayerTurn() && 
            Map.inCircle(GameController.currPlayerPos(), curr_pos, GameController.currPlayerController().moveRadius) &&
            Map.inAStar(GameController.currPlayerPos(), curr_pos, GameController.currPlayerController().moveRadius)
           ) {
            Pathfinding pathfinding = new Pathfinding();
            currPath = pathfinding.FindPath(GameController.currPlayerPos(), curr_pos);
            foreach (Node node in currPath)
                node.getTile().illuminate();
        }
    }
    private void OnMouseExit()
    {
        if (currPath != null)
        foreach (Node node in currPath)
            node.getTile().show();
    }

    // Checks tiletypes to see if that tile is able to be walked on
    private bool isWalkable () {
        foreach (int i in TileTypes.walkAble)
            if (i == id)
                return true;
        return false;
    }
}
