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

    public static int currState = (int)GameState.Player_One;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is it a specific players turn?
    public static bool isPlayerTurn(int id) {
        Debug.Log("CURR STATE: " + currState);
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


    // Turns the turn to the enemy turn
    public static void enemyTurn()
    {
        GameController.currState = (int)GameController.GameState.Enemy;
    }

    public static void nextTurn () {
        currState++;
    }
}
