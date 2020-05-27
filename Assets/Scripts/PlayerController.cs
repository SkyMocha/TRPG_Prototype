using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Player player; // The actual player master class
    public GameObject playerObject; // The player object in the world
    public SpriteRenderer spriteRenderer; // The spriterenderer

    public int id; // The player's ID
    public Vector3 curr_pos; // The players current Vector3 Position

    public int order; // The players turn in the turn order
    Entity entity; // The players entity object

    private void Start()
    {
        //player.moveSphereObject.transform.localScale = new Vector3(5 * 3 / 5, 5 * 3 / 5);
    }

    // Update is called once per frame
    void Update()
    {
        actionHandler();
    }

    // Updates the players position
    public void updatePosition (Vector3 pos) {
        Debug.Log("MOVING PLAYER " + id);
        StartCoroutine(moveTo(pos));
    }

    // Moves a player over time
    public IEnumerator moveTo (Vector3 pos) {
        if (GameController.isPlayerTurn(id) && Map.inCircle(curr_pos, pos, entity.getMoveRadius()) && Map.inAStar(curr_pos, pos, entity.getMoveRadius()))
        {
            Pathfinding pathfinding = new Pathfinding();
            List<Node> path = pathfinding.FindPath(curr_pos, pos);

            curr_pos = pos;
            GameController.setAnimation();
            foreach (Node node in path) {
                playerObject.transform.position = new Vector3(Map.toUnit(node.x), Map.toUnit(node.y), -3);
                Map.updateFogOfWar();
                yield return new WaitForSeconds(0.2f);
            }
            GameController.cancelAnimation();
            GameController.nextTurn();
        }
    }

    // Checks to see if the player can perform an action
    public bool isActionable () {
        return GameController.isPlayerTurn(id) || GameController.isShooting();
    }

    // Handles the actions the player can do
    public void actionHandler () {
        if (isActionable())
        {
            // Does the player want to shoot?
            if (Input.GetKeyDown(KeyCode.E) && !GameController.isShooting())
            {
                Map.drawRadius(curr_pos, entity.getCurrentWeapon().getRange());
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

    // Destorys the player
    public void destroyPlayer () {
        entity.kill();
        Destroy(playerObject);
        UI.destroyCard(order);
        Logs.addEntry("Player " + name + " down!");
    }

    // OnMouseOver and OnMouseExit
    public void OnMouseOver()
    {
        UI.showEntity(this);
    }
    public void OnMouseExit()
    {
        UI.removeEntityCard();
    }

    // Compares one player to another
    public int CompareTo(Player two)
    {
        if (entity.getInitiative() > two.initiative)
            return 1;
        else if (entity.getInitiative() == two.initiative)
            return 0;
        else
            return -1;
    }

    public bool isDead () {
        return entity.isDead();
    }
  
    // Gets the position of the player
    public Vector3 getPos() {
        return curr_pos;
    }

    public Entity getEntity()
    {
        return entity;
    }
    public void setEntity(Entity e)
    {
        entity = e;
    }

}