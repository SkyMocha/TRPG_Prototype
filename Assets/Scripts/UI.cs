using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public static GameObject entityPrefab;

    public static GameObject[] UI_Entities;

    public static void instantiateEntities()
    {
        entityPrefab = Resources.Load<GameObject>("Prefabs/UI_Entity");
        UI_Entities = new GameObject[Map.turnOrder.Length];
        int x = 25;
        int i = 0;
        foreach (object obj in Map.turnOrder)
        {
            if (isPlayer(obj))
                instantiatePlayer((Player) obj, x, i);
            else if (isEnemy(obj))
                instantiateEnemy((Enemy) obj, x, i);
            x += 100;
        }
    }

    public static void instantiatePlayer(Player player, int x, int i) 
    {
        UI_Entities[i] = Instantiate(entityPrefab, new Vector3(x, 75), Quaternion.identity);
        UI_Entities[i].name = player.update.name;
        Text playerName = UI_Entities[i].transform.GetChild(1).GetComponent(typeof(Text)) as Text;
        playerName.text = player.update.name;
        Image playerSprite = UI_Entities[i].transform.GetChild(2).GetComponent(typeof(Image)) as Image;
        playerSprite.sprite = player.update.spriteRenderer.sprite;

        UI_Entities[i].transform.parent = GameObject.FindWithTag("UI").transform;
        RectTransform rt = UI_Entities[i].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 75, rt.rect.height);
    }

    public static void instantiateEnemy(Enemy enemy, int x, int i)
    {
        UI_Entities[i] = Instantiate(entityPrefab, new Vector3(x, 75), Quaternion.identity);
        UI_Entities[i].name = enemy.getController().name;
        Text enemyName = UI_Entities[i].transform.GetChild(1).GetComponent(typeof(Text)) as Text;
        enemyName.text = enemy.getController().name;
        Image enemySprite = UI_Entities[i].transform.GetChild(2).GetComponent(typeof(Image)) as Image;
        enemySprite.sprite = enemy.getController().spriteRenderer.sprite;

        UI_Entities[i].transform.parent = GameObject.FindWithTag("UI").transform;
        RectTransform rt = UI_Entities[i].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 75, rt.rect.height);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool isPlayer(object obj)
    {
        return obj.GetType() == typeof(Player);
    }
    public static bool isEnemy(object obj)
    {
        return obj.GetType() == typeof(Enemy);
    }
}
