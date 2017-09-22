using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveFactory
{
    Board _board;
    List<Move> moves = new List<Move>();
    Dictionary<Piece.pieceType, System.Action> pieceToFunction = new Dictionary<Piece.pieceType, System.Action>();

    private Piece _piece;
    private Piece.pieceType _type;
    private Piece.playerColor _player;
    private Vector2 _position;

    public MoveFactory(Board board)
    {
        _board = board;
        pieceToFunction.Add(Piece.pieceType.PAWN, _GetPawnMoves);
        pieceToFunction.Add(Piece.pieceType.ROOK, _GetRookMoves);
        pieceToFunction.Add(Piece.pieceType.KNIGHT, _GetKnightMoves);
        pieceToFunction.Add(Piece.pieceType.BISHOP, _GetBishopMoves);
        pieceToFunction.Add(Piece.pieceType.QUEEN, _GetQueenMoves);
        pieceToFunction.Add(Piece.pieceType.KING, _GetKingMoves);
    }

    public List<Move> GetMoves(Piece piece, Vector2 position)
    {
        _piece = piece;
        _type = piece.Type;
        _player = piece.Player;
        _position = position;

        foreach(KeyValuePair<Piece.pieceType, System.Action> p in pieceToFunction)
        {
            if (_type == p.Key)
            {
                p.Value.Invoke();
            }
        }

        return moves;
    }

    void _GetPawnMoves()
    {
        if (_piece.Player == Piece.playerColor.BLACK)
        {
            int limit = _piece.HasMoved ? 2 : 3;
            _GenerateMove(limit, new Vector2(0, 1));

            Vector2 diagLeft = new Vector2(_position.x - 1, _position.y + 1);
            Vector2 diagRight = new Vector2(_position.x + 1, _position.y + 1);
            Tile dl = null;
            Tile dr = null;
            if (_IsOnBoard(diagLeft))
            {
                dl = _board.GetTileFromBoard(diagLeft);
            }
            if (_IsOnBoard(diagRight))
            {
                dr = _board.GetTileFromBoard(diagRight);
            }

            if (dl != null && _ContainsPiece(dl) && _IsEnemy(dl))
            {
                _CheckAndStoreMove(diagLeft);
            }
            if (dr != null && _ContainsPiece(dr) && _IsEnemy(dr))
            {
                _CheckAndStoreMove(diagRight);
            }
        }
        else
        {
            int limit = _piece.HasMoved ? 2 : 3;
            _GenerateMove(limit, new Vector2(0, -1));

            Vector2 diagLeft = new Vector2(_position.x - 1, _position.y - 1);
            Vector2 diagRight = new Vector2(_position.x + 1, _position.y - 1);
            Tile dl = null;
            Tile dr = null;
            if (_IsOnBoard(diagLeft))
            {
                dl = _board.GetTileFromBoard(diagLeft);
            }
            if (_IsOnBoard(diagRight))
            {
                dr = _board.GetTileFromBoard(diagRight);
            }

            if (dl != null && _ContainsPiece(dl) && _IsEnemy(dl))
            {
                _CheckAndStoreMove(diagLeft);
            }
            if (dr != null && _ContainsPiece(dr) && _IsEnemy(dr))
            {
                _CheckAndStoreMove(diagRight);
            }
        }
    }

    void _GetRookMoves()
    {
        _GenerateMove(9, new Vector2(0, 1));
        _GenerateMove(9, new Vector2(0, -1));
        _GenerateMove(9, new Vector2(1, 0));
        _GenerateMove(9, new Vector2(-1, 0));
    }

    void _GetKnightMoves()
    {
        Vector2 move;
        move = new Vector2(_position.x + 2, _position.y + 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x + 2, _position.y - 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 2, _position.y + 1);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 2, _position.y - 1);
        _CheckAndStoreMove(move);

        move = new Vector2(_position.x + 1, _position.y - 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x + 1, _position.y + 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 1, _position.y + 2);
        _CheckAndStoreMove(move);
        move = new Vector2(_position.x - 1, _position.y - 2);
        _CheckAndStoreMove(move);
    }

    void _GetBishopMoves()
    {
        _GenerateMove(9, new Vector2(1, 1));
        _GenerateMove(9, new Vector2(-1, -1));
        _GenerateMove(9, new Vector2(1, -1));
        _GenerateMove(9, new Vector2(-1, 1));
    }

    void _GetKingMoves()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                _CheckAndStoreMove(new Vector2(_position.x + x, _position.y + y));
            }
        }
    }

    void _GetQueenMoves()
    {
        _GetBishopMoves();
        _GetRookMoves();
    }

    void _GenerateMove(int limit, Vector2 direction)
    {
        for (int i = 1; i < limit; i++)
        {
            Vector2 move = _position + direction * i;
            if (_IsOnBoard(move) && _ContainsPiece(_board.GetTileFromBoard(move)))
            {
                if (_IsEnemy(_board.GetTileFromBoard(move)) && _type != Piece.pieceType.PAWN)
                {
                    _CheckAndStoreMove(move);
                }
                break;
            }
            _CheckAndStoreMove(move);
        }
    }

    void _CheckAndStoreMove(Vector2 move)
    {
        if (_IsOnBoard(move) && (!_ContainsPiece(_board.GetTileFromBoard(move)) || _IsEnemy(_board.GetTileFromBoard(move))))
        {
            Move m = new Move();
            m.firstPosition = _board.GetTileFromBoard(_position);
            m.pieceMoved = _piece;
            m.secondPosition = _board.GetTileFromBoard(move);

            if (m.secondPosition != null)
                m.pieceKilled = m.secondPosition.CurrentPiece;

            moves.Add(m);
        }
    }

    bool _IsEnemy(Tile tile)
    {
        if (_player != tile.CurrentPiece.Player)
            return true;
        else
            return false;
    }

    bool _ContainsPiece(Tile tile)
    {
        if (!_IsOnBoard(tile.Position))
            return false;

        if (tile.CurrentPiece != null)
            return true;
        else
            return false;
    }

    bool _IsOnBoard(Vector2 point)
    {
        if (point.x >= 0 && point.y >= 0 && point.x < 8 && point.y < 8)
            return true;
        else
            return false;
    }
}
