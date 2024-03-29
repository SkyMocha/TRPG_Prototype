﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    public static GameObject entityPrefab;

    public static GameObject[] UI_Entities;

    public static GameObject entityCard;
    public static Image entityCardImage;
    public static Text entityCardText;
    public static Text entityCardInventory;

    // Goes through the turn order and instantiates all entities
    public static void instantiateEntities()
    {
        entityCard = GameObject.FindWithTag("Entity_Card"); // The entity card on the right side of the screen
        entityCardText = GameObject.FindWithTag("Entity_Card").transform.GetChild(3).GetComponent(typeof (Text)) as Text;
        entityCardImage = GameObject.FindWithTag("Entity_Card").transform.GetChild(1).GetComponent(typeof(Image)) as Image;
        entityCardInventory = GameObject.FindWithTag("Entity_Card").transform.GetChild(4).GetComponent(typeof(Text)) as Text;
        entityCard.SetActive(false);

        assignOrderValue();
        entityPrefab = Resources.Load<GameObject>("Prefabs/UI_Entity");
        UI_Entities = new GameObject[Map.turnOrder.Length];
        int x = 25;
        int i = 0;
        foreach (object obj in Map.turnOrder)
        {
            if (GameController.isPlayer(obj))
                instantiatePlayer((Player) obj, x, i);
            else if (GameController.isEnemy(obj))
                instantiateEnemy((Enemy) obj, x, i);
            x += 100;
            i++;
        }
    }

    // Instantiates a player in the UI
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

    // Instantiates an unknown enemy, that is later to be found
    public static void instantiateEnemy(Enemy enemy, int x, int i)
    {
        UI_Entities[i] = Instantiate(entityPrefab, new Vector3(x, 75), Quaternion.identity);
        UI_Entities[i].name = enemy.getController().name;

        Text enemyName = UI_Entities[i].transform.GetChild(1).GetComponent(typeof(Text)) as Text;
        enemyName.text = "unkown";

        UI_Entities[i].transform.parent = GameObject.FindWithTag("UI").transform;
        RectTransform rt = UI_Entities[i].GetComponent(typeof(RectTransform)) as RectTransform;
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, rt.rect.width);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 75, rt.rect.height);

        Image enemySprite = UI_Entities[i].transform.GetChild(2).GetComponent(typeof(Image)) as Image;
        enemySprite.gameObject.SetActive(false);
    }

    // Updates a enemy when it is visible
    public static void updateEnemy(EnemyController enemy)
    {
        Text enemyName = UI_Entities[enemy.order].transform.GetChild(1).GetComponent(typeof(Text)) as Text;
        enemyName.text = enemy.name;
        Image enemySprite = UI_Entities[enemy.order].transform.GetChild(2).GetComponent(typeof(Image)) as Image;
        enemySprite.gameObject.SetActive(true);
        enemySprite.sprite = enemy.spriteRenderer.sprite;
        Logs.addEntry("Enemy " + enemy.name + " discovered!");
    }

    public static void destroyCard (int id) {
        updateCardX();
        Destroy(UI_Entities[id]);
    }

    // Assigns each individual entity an order value depending on their order in turnOrder
    // No idea why its in UI
    public static void assignOrderValue()
    {
        int i = 0;
        foreach (object obj in Map.turnOrder)
        {
            if (GameController.isPlayer(obj))
                ((Player)obj).update.order = i;
            else if (GameController.isEnemy(obj))
                ((Enemy)obj).getController().order = i;
            i++;
        }
    }

    // Updates the card order on the screen for when an enemy or player is killed
    public static void updateCardX () {
        int x = 25;
        int i = 0;
        foreach (GameObject obj in UI_Entities)
        {
            if (!isDead(i)) {
                RectTransform rt = obj.GetComponent(typeof(RectTransform)) as RectTransform;
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, x, rt.rect.width);
                rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 75, rt.rect.height);
                x += 100;
            }
            i++;
        }
    }

    // Shows the entity on the right-hand side card
    public static void showEntity (object entity) {
        if (GameController.isEnemyCont(entity))
        {
            EnemyController enemy = (EnemyController)entity;
            entityCardText.text = enemy.getEntity().getHealth() + "\n" + enemy.initiative;
            entityCardImage.sprite = enemy.spriteRenderer.sprite;
            entityCardInventory.text = "EMPTY";
        }
        else if (GameController.isPlayerCont(entity)) 
        {
            PlayerController player = (PlayerController)entity;
            entityCardText.text = player.getEntity().getHealth() + "\n" + player.getEntity().getInitiative();
            entityCardImage.sprite = player.spriteRenderer.sprite;
            entityCardInventory.text = getInventoryText(player.getEntity().getEquippedWeapons());
            
        }
        entityCard.SetActive(true);
    }

    // Gets a string based on the equipped of the inventory
    public static string getInventoryText (List<Item> inv) {
        string weaponText = "Weapons: \n";
        string spellText = "Spells: \n";
        foreach (Item item in inv) {
            if (item.isWeapon())
                weaponText += "* " + item.getName();
            if (item.isSpell())
                spellText += "* " + item.getName();
        }
        if (weaponText == "Weapons: \n")
            weaponText = "";
        if (spellText == "Spells: \n")
            spellText = "";
        return weaponText + "\n" + spellText;
    }

    // Removes the entity card from the screen
    public static void removeEntityCard()
    {
        entityCard.SetActive(false);
    }

    public static bool isDead (int i){
        if (GameController.isEnemy(Map.turnOrder[i]) && ((Enemy)Map.turnOrder[i]).getController().isDead())
            return true;
        return false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

}