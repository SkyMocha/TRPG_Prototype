using UnityEngine;
using System.Collections;

public class Entity
{

    string name;

    int health;
    int initiative;
    int defense;
    int elemDefense;
    bool dead;
    int moveRadius;

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

        setHealth(10);
        setInitiative(0);
        setMoveRadius(6);
    }
}
