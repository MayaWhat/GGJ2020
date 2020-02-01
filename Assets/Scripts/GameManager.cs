using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Deck _startDeck;
    private DrawPile _drawPile;
    private Hand _hand;
    private Enemy _enemy;

    private bool _gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        _hand = FindObjectOfType<Hand>();        
        _drawPile = FindObjectOfType<DrawPile>();
        _enemy = FindObjectOfType<Enemy>();

        _drawPile.Init(_startDeck.GetCards());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) && !_gameStarted)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        _gameStarted = true;
        _hand.DrawHand();
    }

    public void PlayerEndedTurn()
    {
        _hand.DiscardHand();
        _enemy.DoTurn();
        _hand.DrawHand();
    }
}
