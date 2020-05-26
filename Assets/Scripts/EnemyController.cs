using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 curr_pos;
    public int id;

    Color color;
    public SpriteRenderer spriteRenderer;
    bool inView;
    bool discovered;

    public int initiative;
    public int order;

    public Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        updatePos();
        if (GameController.isEnemyTurn() && GameController.isEnemyTurn(order) && !isDead())
            enemyAI();
        else if (GameController.isEnemyTurn() && GameController.isEnemyTurn(order) && isDead())
            GameController.nextTurn();
    }

    void enemyAI()
    {
        GameController.nextTurn();
    }

    void updatePos()
    {
        transform.position = curr_pos;
    }

    // Shows the tile for fog of war purposes
    public void show()
    {
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        inView = true;
        if (!discovered)
            UI.updateEnemy(this);
        discovered = true;
    }

    // Hides the tile for fog of war purposes
    public void hide()
    {
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        inView = false;
    }

    public int CompareTo(Enemy two)
    {
        if (initiative > two.getController().initiative)
            return 1;
        else if (initiative == two.getController().initiative)
            return 0;
        else
            return -1;
    }

    public void OnMouseDown()
    {
        if (GameController.isShooting())
            shootEnemy();
    }

    public void OnMouseOver()
    {
        UI.showEntity(this);
    }
    public void OnMouseExit()
    {
        UI.removeEntityCard();
    }

    // What happens when an enemy gets shot
    // Currently just an insta-kill
    public void shootEnemy() {
        if (enemyInShootingDistance())
            foreach (Item i in GameController.currPlayerController().getEquippedWeapons())
                damage(i.damage, i.elemDamage);
        GameController.cancelShooting();
        GameController.nextTurn();
    }

    public bool enemyInShootingDistance () {
        PlayerController playerShooting = Map.players[GameController.getPrevState()].getController();
        return Map.inCircle(playerShooting.getPos(), curr_pos, playerShooting.getCurrentWeapon().getRange());
    }

    public void damage (int normalAmount, int elemAmount) {
        entity.damage(normalAmount, elemAmount);
        if (entity.getHealth() <= 0)
            destroyEnemy();
    }

    public void destroyEnemy () {
        entity.kill();
        Destroy(gameObject);
        UI.destroyCard(order);
        Logs.addEntry("Enemy " + name + " down");
    }

    public bool isDead () {
        return entity.isDead();
    }

    public Entity getEntity () {
        return entity;
    }

    public void setEntity (Entity t) {
        entity = t;
    }
}