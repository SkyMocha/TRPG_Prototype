using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tile
{
    public class Tile : MonoBehaviour
    {

        public GameObject gameObject;
        Texture2D objectTexture;
        public int id;
        int x, y;
        //Transform transform;
        BoxCollider2D boxCollider;
        TileTrigger triggerScript;
        Color color;
        SpriteRenderer spriteRenderer;
        bool inView = false;

        public Tile(Texture2D texture, int tileIndex, int tx, int ty)
        {
            x = tx;
            y = ty;

            this.id = tileIndex;
            if (tileIndex == -1)
                return;

            gameObject = new GameObject();
            objectTexture = texture;

            gameObject.name = x + ", " + y;
            SpriteRenderer sprite = gameObject.AddComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            sprite.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            gameObject.transform.parent = GameObject.FindWithTag("Map").transform;

            gameObject.transform.position = new Vector3(x * 0.08f, y * 0.08f);

            gameObject.tag = "Tile";

            boxCollider = gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;

            triggerScript = gameObject.AddComponent(typeof(TileTrigger)) as TileTrigger;
            triggerScript.id = id;

            color = gameObject.GetComponent<SpriteRenderer>().color;
            spriteRenderer = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        }

        // Checks tiletypes to see if that tile is able to be walked on
        public bool isWalkable()
        {
            foreach (int i in TileTypes.walkAble)
                if (i == id)
                    return true;
            return false;
        }

        // Shows the tile for fog of war purposes
        public void show () {
            spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
            inView = true;
        }

        // Hides the tile for fog of war purposes
        public void hide () {
            spriteRenderer.color = new Color(color.r, color.g, color.b, 0.5f);
            inView = false;
        }

    }

}