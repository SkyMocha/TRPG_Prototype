using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity
{

    string name;

    int health, maxHealth;
    int initiative;
    int defense;
    int elemDefense;
    bool dead;
    int moveRadius;

    List<Item> inventory;
    List<Item> equipped;

    public Entity () {
        inventory = new List<Item>();
        equipped = new List<Item>();
    }

    public string getName() {
        return name;
    }
    public void setName (string tname) {
        name = tname;
    }

    public int getHealth () {
        return health;
    }
    public void setHealth (int newHealth) {
        health = newHealth;
    }
    public void initHealth (int newHealth) {
        setHealth(newHealth);
        setMaxHealth(newHealth);
    }


    public int getMaxHealth()
    {
        return maxHealth;
    }
    public void setMaxHealth(int newHealth)
    {
        maxHealth = newHealth;
    }

    public int getInitiative()
    {
        return initiative;
    }
    public void setInitiative(int newIni)
    {
        initiative = newIni;
    }

    public int getMoveRadius()
    {
        return moveRadius;
    }
    public void setMoveRadius(int newMR)
    {
        moveRadius = newMR;
    }

    public bool isDead () {
        return dead;
    }

    public List<Item> getInventory () {
        return inventory;
    }
    public void addItem (Item e) {
        equipped.Add(e);
    }
    // Gets all equipped weapons
    public List<Item> getEquippedWeapons()
    {
        List<Item> weapons = new List<Item>();
        foreach (Item i in equipped)
        {
            if (i.isWeapon())
                weapons.Add(i);
        }
        return weapons;
    }
    // Gets the currently used weapon
    public Item getCurrentWeapon()
    {
        return getEquippedWeapons()[0];
    }

    public void damage (int normalAmount, int elemAmount) {
        health -= ((normalAmount - defense) + (elemAmount - elemDefense));
    }
    public void kill () {
        dead = true;
    }


}

public class Elemental : Entity {
    public Elemental () {
        setName("Elemental");

        initHealth(10);
        setInitiative(0);
        setMoveRadius(6);

        addItem(new Firebolt());
    }
}
