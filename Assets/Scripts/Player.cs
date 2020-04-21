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
    public MoveSphere moveSphere;
    public int id;
    public PlayerUpdate update;
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
        playerController.moveSphere = playerController.moveSphereObject.GetComponent(typeof(MoveSphere)) as MoveSphere;

        update = playerObject.AddComponent(typeof(PlayerUpdate)) as PlayerUpdate;
        update.playerObject = playerObject;
        update.id = tId;
        update.player = playerController;
        update.moveRadius = moveRadius;

        update.curr_pos = pos;

        update.spriteRenderer = playerObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

    }

    // Use this for initialization
    void Start()
    {
        moveRadius = 5;
    }

    // Changes the players position if it is their turn
    //public void changePos (Vector3 pos){
        // PASSING ID OF 0 WHEN IT SHOULD BE 3
        //Debug.Log(update.id);
        //if (GameController.isPlayerTurn(update.id) && Map.inCircle (update.curr_pos, pos, update.moveRadius))
        //{
        //    update.curr_pos = pos;
        //    update.updatePosition();
        //    Map.updateFogOfWar();
        //}
    //}

    public GameObject getPlayer () {
        return playerObject;
    }

}
