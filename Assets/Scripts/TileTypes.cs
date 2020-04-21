using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTypes
{
    ///* TILE DESCRIPTIONS
    // * 11, 12, 13, 19, 20, 21 = Trees
    // * 33, 41, 42, 43 = Walls
    // * 44, 45, 46 = Water Pits
    // * 
    // */
    //public static int[] nonTravelable = { 11, 12, 13, 19, 20, 21, 33, 41, 42, 43, 44, 45, 46 };

    /* TILE DESCRIPTIONS
     * 92, 93, 94, 157, 158 = Grass
     * 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 14, 15, 16, 18, 22, 23, 24, 26, 27, 28, 29, 30, 31, 32, 34, 35, 36, 37, 38, 39, 40 = Road
     * 125, 126, 133, 134 = Rails
     */
    public static int[] walkAble = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 14, 15, 16, 18, 22, 23, 24, 26, 27, 28, 29, 30, 31, 32, 34, 35, 36, 37, 38, 39, 40, 92, 93, 94, 125, 126, 133, 134, 157, 158 };
}
