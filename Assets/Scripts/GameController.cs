using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Game States
    public enum GameState
    {
        Player_One,
        Player_Two,
        Player_Three,
        Player_Four,
        Enemy,
        Shooting,
    }

    public static int currState = 0; // The current playable or non playable game state
    public static int currTurn = 0; // The current turn of battle
    public static int prevState = 0; // The previous game state the game was in

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdatable())
        {
            if (currTurn < Map.turnOrder.Length && isPlayer(Map.turnOrder[currTurn]))
            {
                updateCurrState((Player)Map.turnOrder[currTurn]);
            }
            else if (currTurn < Map.turnOrder.Length && isEnemy(Map.turnOrder[currTurn]))
                currState = (int)GameState.Enemy;
        }
    }
    bool isUpdatable() {
        return (isEnemyTurn() || isPlayerTurn()) && !isShooting();
    }

    void updateCurrState(Player p)
    {
        currState = p.update.id;
    }

    // Is it a specific players turn?
    public static bool isPlayerTurn(int id)
    {
        return currState == id;
    }

    // Is it any of the players turns?
    public static bool isPlayerTurn()
    {
        return currState == (int)GameState.Player_One
                                           || currState == (int)GameState.Player_Two
                                           || currState == (int)GameState.Player_Three
                                           || currState == (int)GameState.Player_Four;
    }

    bool isPlayer(object obj)
    {
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

    public static bool isEnemyTurn()
    {
        return currState == (int)GameState.Enemy;
    }
    public static bool isEnemyTurn(int id)
    {
        return currTurn == id;
    }

    // Adds one to the turn counter, unless its the final enemies turn in which it resets to 0
    // Need to add contingency for if all enemies and players are dead
    public static void nextTurn()
    {
        currTurn++;
        if (currTurn == Map.turnOrder.Length)
        {
            Logs.addEntry("Player turn");
            currTurn = 0;
        }
        else if (currState == (int)GameState.Enemy)
            Logs.addEntry("Enemy Turn");
        if (UI.isEnemy(Map.turnOrder[currTurn]) && ((Enemy)Map.turnOrder[currTurn]).getController().isDead())
            currTurn++;
    }

    // Sets the current state to shooting while saving the previous state
    public static void setShooting()
    {
        prevState = currState;
        currState = (int)GameState.Shooting;
    }
    public static bool isShooting()
    {
        return currState == (int)GameState.Shooting;
    }
    // Reverts back to the previous state
    public static void cancelShooting()
    {
        currState = prevState;
        Map.updateFogOfWar();
    }

    public static int getPrevState() {
        return prevState;
    }
}