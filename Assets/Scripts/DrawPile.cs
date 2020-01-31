using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPile : MonoBehaviour
{    
    [SerializeField]
    private List<Card> _startingCards;

    [SerializeField]
    private DiscardPile _discardPile;

    private Hand _hand;

    [SerializeField]
    private UIText _uitext;

    public int CardCount { 
        get {
            return transform.childCount;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        _hand = FindObjectOfType<Hand>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUIText();
        _discardPile.UpdateUIText();
    }

    public void Init(List<Card> cards)
    {
        _startingCards = cards;

        _startingCards = Shuffle(_startingCards);
        
        foreach (var card in _startingCards)
        {
            var cardObject = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            cardObject.transform.SetParent(transform, false);
        }

        UpdateUIText();
        _discardPile.UpdateUIText();
    }

    public List<Card> Shuffle(List<Card> cards)
    {
        return cards.OrderBy(a => Random.Range(0, 1000)).ToList();
    }

    public List<Transform> DrawCards(int drawAmount)
    {
        if (CardCount == 0)
        {
            GrabDiscardPile();
        }

        var drawnCards = new List<Transform>();

        var cardsInDrawPile = new List<Transform>();
        foreach (Transform card in transform)
        {
            cardsInDrawPile.Add(card);
        }

        for (var i = 0; i < drawAmount; i++)
        {
            var drawnCard = cardsInDrawPile.FirstOrDefault();

            if (drawnCard == null)
            {
                GrabDiscardPile();
            }

            drawnCards.Add(drawnCard);
            cardsInDrawPile.Remove(drawnCard);
        }

        UpdateUIText();

        return drawnCards;
    }

    private void GrabDiscardPile()
    {
        var cardInDiscardPile = new List<Transform>();
        foreach (Transform card in _discardPile.transform)
        {
            cardInDiscardPile.Add(card);
        }
        
        foreach (var card in cardInDiscardPile)
        {
            card.SetParent(transform, false);
        }

        UpdateUIText();
        _discardPile.UpdateUIText();

        Debug.Log("Discard pile shuffled into draw pile.");
    }

    public void UpdateUIText() {
        _uitext.SetText(CardCount.ToString());
    }
}
