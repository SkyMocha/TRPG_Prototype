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
        Animation,
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
        if (isEnemy(Map.turnOrder[currTurn]) && ((Enemy)Map.turnOrder[currTurn]).getController().isDead())
            currTurn++;
        else if (isPlayer(Map.turnOrder[currTurn]) && ((Player)Map.turnOrder[currTurn]).update.isDead())
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

    public static void setAnimation () {
        prevState = currState;
        currState = (int)GameState.Animation;
    }
    public static void cancelAnimation()
    {
        currState = prevState;
    }
    public static bool isAnimation()
    {
        return currState == (int)GameState.Animation;
    }

    public static int getPrevState() {
        return prevState;
    }

    public static Player getCurrPlayer () {
        return Map.players[(int)currState];
    }
    public static PlayerUpdate currPlayerController()
    {
        return getCurrPlayer().getController();
    }
    public static Vector3 currPlayerPos () {
       return getCurrPlayer().getController().curr_pos;
    }

    public static bool isPlayer(object obj)
    {
        return obj.GetType() == typeof(Player);
    }
    public static bool isEnemy(object obj)
    {
        return obj.GetType() == typeof(Enemy);
    }
    public static bool isEnemyCont(object obj)
    {
        return obj.GetType() == typeof(EnemyController);
    }
    public static bool isPlayerCont(object obj)
    {
        return obj.GetType() == typeof(PlayerUpdate);
    }
}