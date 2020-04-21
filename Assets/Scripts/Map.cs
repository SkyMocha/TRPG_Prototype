using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Tile;

public class Map : MonoBehaviour
{

    Texture2D map_image;
    [SerializeField] int map_number;
    [SerializeField] Vector3[] playerStarts;
    public static int tileSize = 8;
    Texture2D[,] tileTextures;
    int tile_sheet_size = 164;
    public static Tile[,] tiles;
    Sprite[] spriteSheetSprites;
    Texture2D[] spriteSheetTextures;
    public static Player[] players;
    public static Enemy[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        map_number = 0;
        playerStarts = new[] { new Vector3(1, 1, -3), new Vector3(2, 2, -3), new Vector3(3, 1, -3), new Vector3(4, 2, -3) };
        players = new Player[4];
        startsToCoords();

        enemies = new Enemy[1];

        map_image = Resources.Load<Texture2D>("Maps/" + map_number);
        spriteSheetSprites = Resources.LoadAll<Sprite>("TileMaps/galletcity_tiles");
        spriteSheetTextures = spriteSheetToTextures(spriteSheetSprites);
        loadMapTiles();
        loadEntities();
    }

    void startsToCoords()
    {
        for (int i = 0; i < playerStarts.Length; i++)
        {
            playerStarts[i].x *= 0.08f;
            playerStarts[i].y *= 0.08f;
        }
    }

    // Loops through all of the 8x8 sections of the map and creates a texture object for each 8x8 region
    void loadMapTiles()
    {
        tileTextures = new Texture2D[map_image.width / tileSize, map_image.height / tileSize]; // Creates a list of all the tile textures
        tiles = new Tile[map_image.width / tileSize, map_image.height / tileSize]; // Creates a list of all the tiles
        // Loops through the whole map in 8x8 chunks
        for (int x = 0; x < map_image.width; x += tileSize)
        {
            for (int y = 0; y < map_image.height; y += tileSize)
            {
                // Loops through the individual 8x8 region
                Texture2D currTexture = new Texture2D(tileSize, tileSize, TextureFormat.ARGB32, false);
                for (int tile_x = 0; tile_x < tileSize; tile_x++)
                {
                    for (int tile_y = 0; tile_y < tileSize; tile_y++)
                    {
                        int curr_x = x + tile_x;
                        int curr_y = y + tile_y;
                        currTexture.SetPixel(tile_x, tile_y, map_image.GetPixel(curr_x, curr_y));
                    }
                }
                currTexture.Apply(); // applies the changed setpixel values
                matchTexture(x / tileSize, y / tileSize, currTexture);
            }
        }
    }

    // Checks to see if the texture matches with any of x existing textures
    void matchTexture(int x, int y, Texture2D texture)
    {
        for (int i = 0; i < 165; i++)
            if (match(texture, i))
            {
                //Debug.Log("TILE " + x + ", " + y + " IS INDEX OF " + i);
                tiles[x, y] = new Tile(texture, i, x, y);
                return;
            }
        tiles[x, y] = new Tile(null, -1, x, y);
    }

    // Bypasses the matching instead of matchTexture for testing purposes
    void bypassMatching(int x, int y, Texture2D texture)
    {
        tiles[x, y] = new Tile(texture, -1, x, y);
    }

    // Checks to see if one texture matches with i indexed texture.
    bool match(Texture2D one, int index)
    {
        Texture2D two = spriteSheetTextures[index];
        //Debug.Log (one.GetPixel(0, 0) + ", " + two.GetPixel(0, 0));
        for (int x = 0; x < tileSize; x++)
        {
            for (int y = 0; y < tileSize; y++)
            {
                //Debug.Log(x + ", " + y);
                if (one.GetPixel(x, y) != two.GetPixel(x, y))
                    return false;
            }
        }
        return true;
    }

    // Turns the sprite sheet into a list of Texture2Ds
    Texture2D[] spriteSheetToTextures(Sprite[] spriteSheet)
    {
        Texture2D[] texts = new Texture2D[spriteSheet.Length];
        for (int i = 0; i < spriteSheet.Length; i++)
            texts[i] = spriteToTexture2D(spriteSheet[i]);
        return texts;
    }

    // Turns an individual sprite into a Texture2D
    Texture2D spriteToTexture2D(Sprite sprite)
    {
        Texture2D texture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    // Gets a random valid position on the map
    Vector3 randomPos()
    {
        int x = (int)Random.Range(10, tiles.GetLength(0));
        int y = (int)Random.Range(10, tiles.GetLength(1));
        if (tiles[x, y].id == -1)
            return randomPos();
        if (tiles[x, y].isWalkable())
            return new Vector3(x * 0.08f, y * 0.08f, -3);
        else
            return randomPos();
    }

    // Loads all entities
    void loadEntities()
    {
        players[0] = new Player(playerStarts[0], 0);
        players[1] = new Player(playerStarts[1], 1);
        players[2] = new Player(playerStarts[2], 2);
        players[3] = new Player(playerStarts[3], 3);
        enemies[0] = new Enemy(randomPos());

        updateFogOfWar();
    }

    // Checks to see if one Vector3 is inside the circle of another Vector3
    public static bool inCircle(Vector3 center, Vector3 target, int radius)
    {
        return Mathf.Pow(toCoord(target.x) - toCoord(center.x), 2) + Mathf.Pow(toCoord(target.y) - toCoord(center.y), 2) <= Mathf.Pow(radius, 2);
    }

    // Checks to see if all unitys are within a circle
    public static bool inCircleForAll (Vector3 target, int radius) {
        foreach (Player p in players) {
            if (!inCircle(p.update.curr_pos, target, radius))
                return false;
        }
        return true;
    }

    // Goes from a unit in Unity to coords on the tiles
    public static float toCoord (float i)
    {
        return i * 12.5f;
    }
    // Goes from a coord on the coords to a unit in Unity
    public static float toUnit(float i){
        return i * 0.08f;
    }

    // Updates the fog of war for all entities
    public static void updateFogOfWar () {
        foreach (Tile t in tiles)
        {
            if (t.id != -1)
            {
                if (inCircleForAll(t.gameObject.transform.position, 12))
                {
                    t.show();
                }
                else
                {
                    t.hide();
                }
            }
        }
        foreach (Enemy e in enemies)
        {
            if (inCircleForAll(e.getController().curr_pos, 12))
            {
                e.getController().show();
            }
            else
            {
                e.getController().hide();
            }
        }
    }
}