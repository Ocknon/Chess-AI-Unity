using UnityEngine;
using System.Collections;

public class Move
{
    public Tile firstPosition = null;
    public Tile secondPosition = null;
    public Piece pieceMoved = null;
    public Piece pieceKilled = null;
    public int score = -100000000;
}
