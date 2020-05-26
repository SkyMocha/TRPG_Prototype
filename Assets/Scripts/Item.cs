using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Weapon,
    Armor,
    Spell
}

public class Item
{

    public string name;
    public int damage, elemDamage;
    public int type;
    public int defense, elemDefense;
    public int range;

    public void setName(string tname) {
        name = tname;
    }
    public string getName () {
        return name;
    }

    public int getDamage () {
        return damage;
    }
    public int getElemDamage()
    {
        return elemDamage;
    }
    public int getDefense()
    {
        return defense;
    }
    public int getElemDefense()
    {
        return elemDefense;
    }
    public int getRange () {
        return range;
    }

    public bool isWeapon () {
        return type == (int)ItemType.Weapon;
    }
    public void setWeapon () {
        type = (int)ItemType.Weapon;
    }

    public bool isSpell () {
        return type == (int)ItemType.Spell;
    }
    public void setSpell () {
        type = (int)ItemType.Spell;
    }

}

public class Pistol : Item
{
    public Pistol()
    {
        setName("Pistol");
        damage = 5;
        setWeapon();
        range = 7;
    }
}

public class Firebolt : Item
{
    public Firebolt()
    {
        setName("Firebolt");
        elemDamage = 4;
        setSpell();
        range = 10;
    }
}