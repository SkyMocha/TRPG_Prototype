using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 curr_pos;
    public int id;

    Color color;
    SpriteRenderer spriteRenderer;
    bool inView;

    // Start is called before the first frame update
    void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
        spriteRenderer = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    // Update is called once per frame
    void Update()
    {
        updatePos();
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
        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        inView = false;
    }
}
