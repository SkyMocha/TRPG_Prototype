using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Game States
    public enum GameState {
        Player_One,
        Player_Two,
        Player_Three,
        Player_Four,
        Enemy
    }

    public static int currState = 0; // The current game state, only has players and enemy (not specific enemies)
    public static int currTurn = 0; // The current turn of battle

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currTurn < Map.turnOrder.Length && isPlayer(Map.turnOrder[currTurn]))
        {
            updateCurrState((Player)Map.turnOrder[currTurn]);
        }
        else if (currTurn < Map.turnOrder.Length && isEnemy(Map.turnOrder[currTurn]))
            currState = (int)GameState.Enemy;
    }

    void updateCurrState (Player p) {
        currState = p.update.id;
    }

    // Is it a specific players turn?
    public static bool isPlayerTurn(int id) {
        Debug.Log("CURR STATE: " + currState);
        Debug.Log("ID PASSED: " + id);
        return currState == id;
    }

    // Is it any of the players turns?
    public static bool isPlayerTurn()
    {
        return currState == (int) GameState.Player_One
                                           || currState == (int)GameState.Player_Two
                                           || currState == (int)GameState.Player_Three
                                           || currState == (int)GameState.Player_Four;
    }

    bool isPlayer (object obj) {
        return obj.GetType() == typeof(Player);
    }
    bool isEnemy(object obj)
    {
        return obj.GetType() == typeof(Enemy);
    }

    // Turns the turn to the enemy turn
    public static void enemyTurn()
    {
        GameController.currState = (int)GameController.GameState.Enemy;
    }

    public static bool isEnemyTurn () {
        return currState == (int)GameState.Enemy;
    }

    public static void nextTurn () {
        currTurn++;
    }
}
