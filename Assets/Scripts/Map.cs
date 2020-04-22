using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Game.Tile;

public class Map : MonoBehaviour
{

    Texture2D map_image;
    [SerializeField] int map_number;
    [SerializeField] Vector3[] playerStarts;
    public static int tileSize = 8;
    Texture2D[,] tileTextures;
    public static Tile[,] tiles;
    Sprite[] spriteSheetSprites;
    Texture2D[] spriteSheetTextures;
    public static Player[] players;
    public static Enemy[] enemies;
    public static object[] turnOrder;
    public Logs logs;

    // Start is called before the first frame update
    void Start()
    {
        map_number = 0;
        playerStarts = new[] { new Vector3(1, 1, -3), new Vector3(2, 2, -3), new Vector3(3, 1, -3), new Vector3(4, 2, -3) };
        players = new Player[4];
        startsToCoords();

        enemies = new Enemy[4];

        map_image = Resources.Load<Texture2D>("Maps/" + map_number);
        spriteSheetSprites = Resources.LoadAll<Sprite>("TileMaps/galletcity_tiles");
        spriteSheetTextures = spriteSheetToTextures(spriteSheetSprites);
        loadMapTiles();
        loadEntities();
        updateFogOfWar();
        UI.instantiateEntities();
        logs = new Logs();
        //Logs.addEntry("As you enter the city propers through the road you encounter nothing more than a desolate city, once seeming to bustle with life.");
        Logs.addEntry("As you enter the city propers through the road you encounter nothing more than a desolate city, " +
                      "once seeming to bustle with life." +
                      "Ooze eminates from the roads as though clearly something has happened to this once great city." +
                      "You choose to begin your investigation of this metropolis, guns drawn.");
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
        int x = (int)UnityEngine.Random.Range(10, tiles.GetLength(0));
        int y = (int)UnityEngine.Random.Range(10, tiles.GetLength(1));
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
        enemies[1] = new Enemy(randomPos());
        enemies[2] = new Enemy(randomPos());
        enemies[3] = new Enemy(randomPos());

        turnOrder = getTurnOrder();
        printTurnOrder();
    }

    // Checks to see if one Vector3 is inside the circle of another Vector3
    public static bool inCircle(Vector3 center, Vector3 target, int radius)
    {
        return Mathf.Pow(toCoord(target.x) - toCoord(center.x), 2) + Mathf.Pow(toCoord(target.y) - toCoord(center.y), 2) <= Mathf.Pow(radius, 2);
    }

    // Checks to see if all unitys are within a circle
    public static bool inCircleForAll(Vector3 target, int radius)
    {
        foreach (Player p in players)
        {
            if (inCircle(p.update.curr_pos, target, radius))
                return true;
        }
        return false;
    }

    // Goes from a unit in Unity to coords on the tiles
    public static float toCoord(float i)
    {
        return i * 12.5f;
    }
    // Goes from a coord on the coords to a unit in Unity
    public static float toUnit(float i)
    {
        return i * 0.08f;
    }

    // Updates the fog of war for all entities
    public static void updateFogOfWar()
    {
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
            if (!e.getController().isDead() && inCircleForAll(e.getController().curr_pos, 12))
            {
                e.getController().show();
            }
            else if (!e.getController().isDead())
            {
                e.getController().hide();
            }
        }
    }

    // Gets the turn order based on initiative
    public static object[] getTurnOrder()
    {
        object[] entities = new object[players.Length + enemies.Length];
        Player[] sortedPlayers = newPlayerArray(players);
        Array.Sort(sortedPlayers, new Comparison<Player>((p1, p2) => p2.update.CompareTo(p1)));
        Enemy[] sortedEnemies = newEnemyArray(enemies);
        Array.Sort(sortedEnemies, new Comparison<Enemy>((p1, p2) => p2.getController().CompareTo(p1)));
        entities = mergeArrays(sortedPlayers, sortedEnemies);
        return entities;
    }

    // Used to copy an existing player array onto a new player array for get turn order
    public static Player[] newPlayerArray(Player[] p)
    {
        Player[] newP = new Player[p.Length];
        for (int i = 0; i < p.Length; i++)
        {
            newP[i] = p[i];
        }
        return newP;
    }
    // Used to copy an existing enemy array onto a new enemy array for get turn order
    public static Enemy[] newEnemyArray(Enemy[] e)
    {
        Enemy[] newE = new Enemy[e.Length];
        for (int i = 0; i < e.Length; i++)
        {
            newE[i] = e[i];
        }
        return newE;
    }

    // Prints the turn order for debugging
    public static void printTurnOrder()
    {
        string s = "";
        foreach (object obj in turnOrder)
        {
            s += obj.GetType() + " ";
        }
    }

    // Shamelessly taken (and modified obviously) from GeeksforGeeks
    public static object[] mergeArrays(Player[] arr1, Enemy[] arr2)
    {
        int i = 0, j = 0, k = 0;
        int n1 = arr1.Length;
        int n2 = arr2.Length;
        object[] arr3 = new object[n1 + n2];

        while (i < n1 && j < n2)
        {
            if (arr1[i].update.initiative > arr2[j].getController().initiative)
                arr3[k++] = arr2[j++];
            else
                arr3[k++] = arr1[i++];
        }

        while (i < n1)
            arr3[k++] = arr1[i++];

        while (j < n2)
            arr3[k++] = arr2[j++];

        return arr3;
    }

    // Draws a radius around a specific point
    public static void drawRadius(Vector3 pos) {
        foreach (Tile t in tiles)
        {
            if (t.id != -1)
            {
                if (inCircle(pos, t.gameObject.transform.position, 8))
                {
                    t.illuminate();
                }
            }
        }
    }
}