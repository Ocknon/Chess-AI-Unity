using UnityEngine;
using System.Collections;

public class Tile
{
    private Vector2 _position = Vector2.zero;
    public Vector2 Position
    {
        get { return _position; }
    }

    private Piece _currentPiece = null;
    public Piece CurrentPiece
    {
        get { return _currentPiece; }
        set { _currentPiece = value; }
    }

    public Tile(int x, int y)
    {
        _position.x = x;
        _position.y = y;

        if (y == 0 || y == 1 || y == 6 || y == 7)
        {
            _currentPiece = GameObject.Find(x.ToString() + " " + y.ToString()).GetComponent<Piece>();
        }
    }

    public void SwapFakePieces(Piece newPiece)
    {
        _currentPiece = newPiece;
    }
}
