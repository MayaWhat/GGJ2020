using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    private EnemyDrawPile _drawPile;
    private EnemyDiscardPile _discardPile;
    [SerializeField]
    private List<EnemyCard> _cards;
    private int _defaultHandSize = 2;
    private int _handSize;

    // Start is called before the first frame update
    void Start()
    {
        _handSize = _defaultHandSize;
        _drawPile = FindObjectOfType<EnemyDrawPile>();
        _discardPile = FindObjectOfType<EnemyDiscardPile>();
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
}
