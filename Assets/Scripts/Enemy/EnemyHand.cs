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
        for (int i = 0; i < _handSize; i++)
        {
            var card = _drawPile.DrawCard();
            card.transform.SetParent(transform, false);
        }

        // Add a placeholder in the middle to push cards to either side of enemy (hax)
        var placeholder = Instantiate(new GameObject());
        placeholder.AddComponent<RectTransform>();
        placeholder.transform.SetParent(transform, false);
        placeholder.transform.SetSiblingIndex(transform.childCount / 2);

        Debug.Log($"Enemy hand drawn. {_handSize} cards.");
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
        Debug.Log("Enemy Hand discarded.");
    }

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
    }

    public void PlayAllCards() 
    {
        var cardsToPlay = new List<Transform>();

        foreach(Transform cardObject in transform)
        {
            cardsToPlay.Add(cardObject);
        }

        foreach(Transform cardObject in cardsToPlay)
        {
            var card = cardObject.GetComponent<EnemyCard>();

            if(card == null) {
                continue; // Skip placeholders
            }

            card.PlayMe();
        }

        // Destroy placeholders
        foreach(Transform cardObject in transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }
    }
}
