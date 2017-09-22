using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour
{
    public Move move;
    GameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && move != null)
        {
            manager.SwapPieces(move);
        }
    }
}
