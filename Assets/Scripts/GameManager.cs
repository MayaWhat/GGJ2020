using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Deck _startDeck;
    private DrawPile _drawPile;
    private Hand _hand;
    // Start is called before the first frame update
    void Start()
    {
        _hand = FindObjectOfType<Hand>();        
        _drawPile = FindObjectOfType<DrawPile>();

        _drawPile.Init(_startDeck.GetCards());
        _hand.DrawHand();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
