using UnityEngine;
using System.Collections;

public class Board
{
    private static Board _instance = null;
    public static Board Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Board();
            }
            return _instance;
        }
    }

    private Tile[,] _board = new Tile[8, 8];

    public void SetupBoard()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                _board[x, y] = new Tile(x, y);
            }
        }
    }

    public Tile GetTileFromBoard(Vector2 tile)
    {
        return _board[(int)tile.x, (int)tile.y];
    }
}
