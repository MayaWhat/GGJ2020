using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : MonoBehaviour
{
    private EnemyDrawPile _drawPile;
    private EnemyDiscardPile _discardPile;
    [SerializeField]
    private List<EnemyCard> _cards;
    protected Canvas _canvas;

    private int _defaultHandSize = 2;
    private int _handSize;

    // Start is called before the first frame update
    void Start()
    {
        _handSize = _defaultHandSize;
        _drawPile = FindObjectOfType<EnemyDrawPile>();
        _discardPile = FindObjectOfType<EnemyDiscardPile>();
        _canvas = FindObjectOfType<Canvas>();     
    }

    public void DrawHand(Action onFinish)
    {
        // Remove any placeholders
        foreach(Transform cardObject in transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }

        var cards = new RectTransform[_handSize];
        for (int i = 0; i < _handSize; i++)
        {
            var card = _drawPile.DrawCard();
            card.transform.SetParent(transform, false);
            card.localPosition = new Vector3(0f, 0f, 0f);
            cards[i] = card;
        }

        StartCoroutine(DrawHandAnimate(cards, onFinish));

        Debug.Log($"Enemy hand drawn. {_handSize} cards.");
    }

    IEnumerator DrawHandAnimate(RectTransform[] cards, Action onFinish)
    {
        for (var i = 0; i < cards.Length; i++)
        {
            var multiplier = (i % 2) == 0 ? -1 : 1;

            GameManager.Instance.Sounds.EnemyDrawCard.Play();
            var no = i / 2;
            var xMoveTime = 0.06f * (no + 1);
            var destinationPosition = (160f + (no * 90f)) * multiplier;

            if(xMoveTime > 0f)
            {
                for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / xMoveTime)
                {
                    var x = destinationPosition * Mathf.Min(t, 1.0f);
                    cards[i].localPosition = new Vector3(x, 0f, 0f);

                    yield return null;
                }
            }

            cards[i].localPosition = new Vector3(destinationPosition, 0f, 0f);

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.2f);

        onFinish();
    }

    public void DiscardHand()
    {
        var cardsInHand = new List<Transform>();
        foreach (Transform card in transform)
        {
            cardsInHand.Add(card);
        }

        foreach (var cardObject in cardsInHand)
        {
            var card = cardObject.GetComponent<EnemyCard>();

            if(card == null) {
                continue; // Skip placeholders
            }

            _discardPile.Add(cardObject);
        }

        // Remove any placeholders
        foreach(Transform cardObject in transform)
        {
            GameObject.Destroy(cardObject.gameObject);
        }

        Debug.Log("Enemy Hand discarded.");
    }

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
    }

    public void PlayAllCards(Action whenDone) 
    {
        var cardsToPlay = new Queue<EnemyCard>();

        foreach(Transform cardObject in transform)
        {
            cardsToPlay.Enqueue(cardObject.GetComponent<EnemyCard>());
        }

        StartCoroutine(PlayCard(cardsToPlay, whenDone));
    }


    private IEnumerator PlayCard(Queue<EnemyCard> cards, Action whenDone)
    {
        var card = cards.Dequeue();

        var position = card.transform.position;
        card.transform.SetParent(_canvas.transform);
        card.transform.position = position;

        var startPosition = card.transform.localPosition;       

        var moveTime = 0.1f;

        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
        {
            var movement = -80f * t;
            card.transform.localPosition = startPosition + new Vector3(0f, movement, 0f);

            yield return null;
        }

        startPosition += new Vector3(0f, -80f, 0f);
        card.transform.localPosition = startPosition;

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(GameManager.Instance.Enemy.AnimateAction());

        yield return new WaitForSeconds(0.2f);

        card.PlayMe();

        yield return new WaitForSeconds(1f);

        if(cards.Count == 0)
        {
            whenDone();
        }
        else
        {
            StartCoroutine(PlayCard(cards, whenDone));
        }
    }
}
