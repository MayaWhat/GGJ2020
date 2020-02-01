using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private DrawPile _drawPile;
    private DiscardPile _discardPile;
    [SerializeField]
    private List<Card> _cards;
    private int _defaultHandSize = 5;
    private int _handSize;

    // Start is called before the first frame update
    void Start()
    {
        _handSize = _defaultHandSize;
        _drawPile = FindObjectOfType<DrawPile>();
        _discardPile = FindObjectOfType<DiscardPile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawHand()
    {
        var drawnCards = _drawPile.DrawCards(_handSize);

        foreach (var card in drawnCards)
        {
            card.transform.SetParent(transform, false);
        }
        Debug.Log($"Hand drawn. {_handSize} cards.");
    }

    public void DiscardHand()
    {
        var cardsInHand = new List<Transform>();
        foreach (Transform card in transform)
        {
            cardsInHand.Add(card);
        }

        foreach (var card in cardsInHand)
        {
            _discardPile.Add(card);
        }
        Debug.Log("Hand discarded.");
    }

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
    }

    public bool CanPlaySomething() {
        foreach(Transform card in transform) {
           if(card.gameObject.GetComponent<Card>().CanBePlayed()) {
               return true;
           }
        }
        return false;
    }
}
