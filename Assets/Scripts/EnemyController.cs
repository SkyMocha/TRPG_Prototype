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

    public int initiative;

    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        updatePos();
        //if (GameController.isEnemyTurn())
    }

    void updatePos () {
        transform.position = curr_pos;
    }

    // Shows the tile for fog of war purposes
    public void show()
    {
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
        inView = true;
    }

    // Hides the tile for fog of war purposes
    public void hide()
    {
        //spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
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
}
