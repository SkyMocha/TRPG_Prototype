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
    public static Node[,] nodeMap;
    Sprite[] spriteSheetSprites;
    Texture2D[] spriteSheetTextures;
    public static Player[] players;
    public static Enemy[] enemies;
    public static object[] turnOrder;
    public Logs logs;
    int x_temp = -1, y_temp = -1, clusterSize = 0, clusterMax = 2;

    // MAP GENERATION

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
        setNodeMap();
        UI.instantiateEntities();
        logs = new Logs();
        Logs.addEntry("As you enter the city propers through the road you encounter nothing more than a desolate city, " +
                      "once seeming to bustle with life. " +
                      "Ooze eminates from the roads as though clearly something has happened to this once great city. " +
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

    // Gets a random valid position on the map, tries to cluster enemies together
    Vector3 randomPos()
    {
        if (x_temp == -1 || clusterSize >= clusterMax)
        {
            x_temp = (int)UnityEngine.Random.Range(10, tiles.GetLength(0));
        }
        else
            x_temp = (int)UnityEngine.Random.Range(clampToMap(x_temp - 2, 2, 0), clampToMap(x_temp + 3, 2, 0));
        if (y_temp == -1 || clusterSize >= clusterMax)
        {
            y_temp = (int)UnityEngine.Random.Range(10, tiles.GetLength(1));
            clusterSize = 0;
        }
        else
            y_temp = (int)UnityEngine.Random.Range(clampToMap(y_temp - 2, 2, 1), clampToMap(y_temp + 3, 2, 1));

        if (tiles[x_temp, y_temp].id == -1)
            return randomPos();
        if (tiles[x_temp, y_temp].isWalkable())
        {
            clusterSize++;
            return new Vector3(x_temp * 0.08f, y_temp * 0.08f, -3);
        }
        else
            return randomPos();
    }

    int clampToMap(int eq, int min, int dimension)
    {
        return Mathf.Clamp(eq, 2, tiles.GetLength(1));
    }

    // Loads all entities
    void loadEntities()
    {
        players[0] = new Player(playerStarts[0], 0);
        players[1] = new Player(playerStarts[1], 1);
        players[2] = new Player(playerStarts[2], 2);
        players[3] = new Player(playerStarts[3], 3);
        enemies[0] = new Enemy(new Elemental(), randomPos());
        enemies[1] = new Enemy(new Elemental(), randomPos());
        enemies[2] = new Enemy(new Elemental(), randomPos());
        enemies[3] = new Enemy(new Elemental(), randomPos());

        Debug.Log("GETTING TURN ORDER");
        turnOrder = getTurnOrder();
        printTurnOrder();
    }

    // MAP UTILITY

    public static Tile GetTile(Vector3 pos)
    {
        return tiles[(int)toCoord(pos.x), (int)toCoord(pos.y)];
    }
    public static Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
    public static void setNodeMap() {
        nodeMap = new Node[tiles.GetLength(0), tiles.GetLength(1)];
        foreach (Tile t in tiles)
            nodeMap[t.x, t.y] = new Node(t.x, t.y, t.isWalkable());

    }
    public static Node GetNode(Vector3 pos)
    {
        return nodeMap[(int)toCoord(pos.x), (int)toCoord(pos.y)];
    }
    public static Node GetNode(int x, int y) {
        return nodeMap[x, y];
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

    // A STAR

    // Checks to see if a point is within A* range for a start position
    public static bool inAStar (Vector3 start, Vector3 end, int moveRadius) {

        Pathfinding pathfinding = new Pathfinding();
        List<Node> path = pathfinding.FindPath(start, end);
        if (path != null && path.Count <= moveRadius + 1)
            return true;

        return false;
    }

    // Gets all players in range from a position
    // Used currently for enemyAI
    public static List<Player> playersInRange (Vector3 start, int radius) {
        List<Player> temp = new List<Player>(); 
        foreach (Player p in players)
            if (inCircle(start, p.getController().getPos(), radius))
                temp.Add(p);
        return temp;
    }

    // MORE UTILITY

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
        Array.Sort(sortedPlayers, new Comparison<Player>((p1, p2) => p2.getController().CompareTo(p1)));
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
        Debug.Log(s);
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
            if (arr1[i].getController().getEntity().getInitiative() > arr2[j].getController().getEntity().getInitiative())
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
    public static void drawRadius(Vector3 pos, int radius) {
        foreach (Tile t in tiles)
        {
            if (t.id != -1)
            {
                if (inCircle(pos, t.gameObject.transform.position, radius))
                {
                    t.illuminate();
                }
            }
        }
    }

    public static Player[] getPlayers () {
        return players;
    }
}