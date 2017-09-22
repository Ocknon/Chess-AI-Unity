using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public enum pieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1};
    public enum playerColor { BLACK, WHITE, UNKNOWN = -1};

    [SerializeField] private pieceType _type = pieceType.UNKNOWN;
    [SerializeField] private playerColor _player = playerColor.UNKNOWN;
    public pieceType Type
    {
        get { return _type; }
    }
    public playerColor Player
    {
        get { return _player; }
    }

    public Sprite pieceImage = null;
    public Vector2 position;
    private Vector3 moveTo;
    private GameManager manager;

    private MoveFactory factory = new MoveFactory(Board.Instance);
    private List<Move> moves = new List<Move>();

    private bool _hasMoved = false;
    public bool HasMoved
    {
        get { return _hasMoved; }
        set { _hasMoved = value; }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && _player == playerColor.WHITE && manager.playerTurn)
        {
            moves.Clear();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }

            moves = factory.GetMoves(this, position);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            i.transform.position = this.transform.position;
        }
    }

    public void MovePiece(Vector3 position)
    {
        moveTo = position;
    }

    void Start()
    {
        moveTo = this.transform.position;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, moveTo, 3 * Time.deltaTime);
    }
}
