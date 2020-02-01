using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private DrawPile _drawPile;
    private DiscardPile _discardPile;
    [SerializeField]
    private List<Card> _cards;

    [SerializeField]
    private FMODUnity.StudioEventEmitter _drawCardSound;


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

    public void DrawHand(Action onFinish)
    {
        var cards = new RectTransform[_handSize];
        for (int i = 0; i < _handSize; i++)
        {
            var card = _drawPile.DrawCard();
            card.SetParent(transform, false);
            card.localPosition = new Vector3(0f, -250f, 0f);
            cards[i] = card;
        }

        StartCoroutine(DrawHandAnimate(cards, onFinish));
        Debug.Log($"Hand drawn. {_handSize} cards.");
    }

    IEnumerator DrawHandAnimate(RectTransform[] cards, Action onFinish)
    {
        var yMoveTime = 0.2f;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / yMoveTime)
        {
            var x = -(340f * Mathf.Min(t, 1.0f));
            var y = -(250f * (1f - Mathf.Min(t, 1.0f)));

            foreach(var card in cards)
            {
                card.localPosition = new Vector3(x, y, 0f);
            }

            yield return null;
        }

        for (var i = cards.Length - 1; i >= 0; i--)
        {
            _drawCardSound.Play();
            var xMoveTime = 0.06f * i;
            if(xMoveTime > 0f)
            {
                for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / xMoveTime)
                {
                    var x = -340f + ((i * 140f) * Mathf.Min(t, 1.0f));

                    cards[i].localPosition = new Vector3(x, 0f, 0f);

                    yield return null;
                }
            }

            cards[i].localPosition = new Vector3(-340f + (i * 140f), 0f, 0f);
        }

        onFinish();
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
