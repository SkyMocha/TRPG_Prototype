using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyTypes {
        Elemental,
    }

    public GameObject enemyPrefab;

    GameObject enemyObject;
    EnemyController enemy;

    public int initiative = 1;

    public bool isPlayer = false;

    public Enemy(Vector3 pos, int tId)
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");

        enemyObject = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemyObject.name = "Enemy_" + tId;

        enemy = enemyObject.AddComponent(typeof(EnemyController)) as EnemyController;

        enemy.id = tId;
        enemy.curr_pos = pos;

        enemy.spriteRenderer = enemyObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

    }

    public Enemy(Vector3 pos)
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");

        enemyObject = Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemyObject.name = "Enemy_" + (int)EnemyTypes.Elemental;

        enemy = enemyObject.AddComponent(typeof(EnemyController)) as EnemyController;

        enemy.id = (int)EnemyTypes.Elemental;
        enemy.curr_pos = pos;

        enemy.spriteRenderer = enemyObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

    }

    public GameObject getEnemy()
    {
        return enemyObject;
    }

    public EnemyController getController () {
        return enemy;
    }

}
