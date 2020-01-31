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
        _cards = _drawPile.DrawCards(_handSize);

        foreach (var card in _cards)
        {
            var cardObject = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            cardObject.transform.SetParent(transform, false);
        }
    }

    public void PlayCard(Card card)
    {
        _discardPile.Add(card);
    }
}
