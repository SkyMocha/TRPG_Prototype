using UnityEngine;
using System.Collections;


/* NOTES:
 * Because Player is a shared class, the update works weirdly
 * therefore all of the logic goes in PlayerUpdate.cs
*/
public class Player : MonoBehaviour
{
    public GameObject playerPrefab;

    Vector3 curr_pos;
    GameObject playerObject;
    Player playerController;
    int moveRadius;
    public GameObject moveSphereObject;
    public int id;
    public PlayerController update;
    public int initiative = 5;
    //bool isPlayer = true;

    public Player(Vector3 pos, int tId) {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player"); // Loads the player prefab

        playerObject = Instantiate(playerPrefab, pos, Quaternion.identity);
        playerObject.name = "Player_" + tId;

        playerController = playerObject.GetComponent(typeof(Player)) as Player;

        playerController.id = tId;
        playerController.curr_pos = pos;

        playerController.moveSphereObject = playerObject.transform.GetChild(0).gameObject;

        update = playerObject.AddComponent(typeof(PlayerController)) as PlayerController;
        update.playerObject = playerObject;
        update.id = tId;
        update.player = playerController;

        update.curr_pos = pos;

        update.spriteRenderer = playerObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        
        loadSprites();

        Entity e = new Entity();
        e.setMoveRadius(5);
        e.setInitiative(5);
        e.initHealth(20);
        e.addItem(new Pistol());
        update.setEntity(e);

    }

    void loadSprites () {
        if (update.id == 0)
            update.spriteRenderer.sprite = Resources.Load<Sprite>("CustomAssets/Players/Player_0_Idle");
    }

    // Use this for initialization
    void Start()
    {
        moveRadius = 5;
    }

    public GameObject getPlayer () {
        return playerObject;
    }
    public PlayerController getController()
    {
        return update;
    }

}
