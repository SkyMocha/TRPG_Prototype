using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject playerObject;
    public int id;
    public Vector3 curr_pos;
    public Player player;
    public int moveRadius;
    public int initiative;
    public int order;
    public int health = 20;
    public SpriteRenderer spriteRenderer;
    public bool dead;

    public List<Item> inventory; // The players whole inventory
    public List<Item> equipped; // The equipped items for the player

    private void Start()
    {
        player.moveSphereObject.transform.localScale = new Vector3(5 * 3 / 5, 5 * 3 / 5);
        moveRadius = 5;
        inventory = new List<Item>();
        equipped = new List<Item>();
        equipped.Add(new Pistol());
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
        if (GameController.isPlayerTurn(id) && Map.inCircle(curr_pos, pos, moveRadius) && Map.inAStar(curr_pos, pos, moveRadius))
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
                Map.drawRadius(curr_pos, getCurrentWeapon().getRange());
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

    // Damages the player
    public void damage (int amount) {
        health -= amount;
        if (health <= 0)
            destroyPlayer();
    }

    // Destorys the player
    public void destroyPlayer () {
        dead = true;
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
        if (initiative > two.initiative)
            return 1;
        else if (initiative == two.initiative)
            return 0;
        else
            return -1;
    }

    public bool isDead () {
        return dead;
    }

    public List<Item> getInventory()
    {
        return inventory;
    }

    // Gets all equipped items
    public List<Item> getEquipped () {
        return equipped;
    }

    // Gets all equipped weapons
    public List<Item> getEquippedWeapons () {
        List<Item> weapons = new List<Item>();
        foreach (Item i in equipped) {
            if (i.isWeapon())
                weapons.Add(i);
        }
        return weapons;
    }

    // Gets the currently used weapon
    public Item getCurrentWeapon () {
        return getEquippedWeapons()[0];
    }

    // Gets the position of the player
    public Vector3 getPos() {
        return curr_pos;
    }

}