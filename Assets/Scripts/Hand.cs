using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private DrawPile _drawPile;
    private DiscardPile _discardPile;
    [SerializeField]
    private List<Card> _cards;
    private int _defaultHandSize = 5;
    private int _handSize;
    private HalfCard _activeHalfCard;

    protected Canvas _canvas;
    protected ScreenFlash _screenFlash;

    // Start is called before the first frame update
    void Start()
    {
        _handSize = _defaultHandSize;
        _drawPile = FindObjectOfType<DrawPile>();
        _discardPile = FindObjectOfType<DiscardPile>();
        _canvas = FindObjectOfType<Canvas>();     
        _screenFlash = FindObjectOfType<ScreenFlash>();
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

        _activeHalfCard = null;
        UpdateHalfCardHighlighting();
        StartCoroutine(DrawHandAnimate(cards, onFinish));
        Debug.Log($"Hand drawn. {_handSize} cards.");
    }

    IEnumerator DrawHandAnimate(RectTransform[] cards, Action onFinish)
    {
        var yMoveTime = 0.2f;
        GameManager.Instance.Sounds.PlayerDrawCard.Play();
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

        foreach(var card in cards)
        {
            card.localPosition = new Vector3(-340f, 0f, 0f);
        }

        for (var i = cards.Length - 1; i >= 0; i--)
        {
            if(i > 0)
            {
                GameManager.Instance.Sounds.PlayerDrawCard.Play();
            }
            
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

            yield return new WaitForSeconds(0.05f);
        }

        onFinish();
    }

    public void DiscardHand()
    {
        _activeHalfCard = null;
        UpdateHalfCardHighlighting();

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

    public bool CanPlaySomething() 
    {
        var existingLeftHalf = false;
        var existingRightHalf = false;

        foreach(Transform card in transform) 
        {
            if(card.gameObject.GetComponent<Card>().CanBePlayed()) 
            {
                return true;
            }

            var existingHalf = card.gameObject.GetComponent<HalfCard>();

            if(existingHalf != null) 
            {
                if(existingHalf.IsLeftHalf) 
                {
                    existingLeftHalf = true;
                }
                else
                {
                    existingRightHalf = true;
                }

                if(existingLeftHalf && existingRightHalf) {
                    return true;
                }
            }
        }
        return false;
    }

    public void FullCardSelected() {
        _activeHalfCard = null;
        UpdateHalfCardHighlighting();
    }

    public void HalfCardSelected(HalfCard halfCard) 
    {
        if(_activeHalfCard == null)
        {
            _activeHalfCard = halfCard;
            UpdateHalfCardHighlighting();
            return;
        }

        if(_activeHalfCard == halfCard)
        {
            _activeHalfCard = null;
            UpdateHalfCardHighlighting();
            return;
        }

        if(halfCard.IsLeftHalf != _activeHalfCard.IsLeftHalf) 
        {
            if(halfCard.IsLeftHalf) 
            {
                Combine(halfCard, _activeHalfCard);
                _activeHalfCard = null;
                UpdateHalfCardHighlighting();
                return;
            }
            Combine(_activeHalfCard, halfCard);
            _activeHalfCard = null;
            UpdateHalfCardHighlighting();
            return;
        }

         _activeHalfCard = halfCard;
         UpdateHalfCardHighlighting();
    }

    private void UpdateHalfCardHighlighting() {
        
        foreach(Transform cardObject in transform)
        {
            var card = cardObject.GetComponent<HalfCard>();
            if(card != null)
            {
                card.ChangeCardBack(false, false);
            }
        }

        if(_activeHalfCard == null)
        {
            return;
        }
        
        _activeHalfCard.ChangeCardBack(true, true);

        foreach(Transform cardObject in transform)
        {
            var halfCard = cardObject.GetComponent<HalfCard>();

            if(halfCard != null) 
            {
                if(_activeHalfCard.IsLeftHalf && !halfCard.IsLeftHalf && _activeHalfCard.Affordable)
                {
                    halfCard.ChangeCardBack(false, true);
                }
                else if(!_activeHalfCard.IsLeftHalf && halfCard.IsLeftHalf && halfCard.Affordable)
                {
                    halfCard.ChangeCardBack(false, true);
                }
            }
        }
    }

    public void Combine(HalfCard leftCard, HalfCard rightCard) 
    {
        leftCard.Clickable = false;
        leftCard.ChangeCardBack(false, false);
        rightCard.Clickable = false;
        rightCard.ChangeCardBack(false, false);
        GameManager.Instance.Busyness++;  

        GameManager.Instance.Sounds.Combine.Play();
        StartCoroutine(AnimateCombineCard(leftCard, true));
        StartCoroutine(AnimateCombineCard(rightCard, false, () =>
        {
            StartCoroutine(AnimateCombination(leftCard, rightCard));
        }));
    }
    
    private IEnumerator AnimateCombineCard(HalfCard card, bool isLeft, Action onComplete = null)
    {

        var position = card.transform.position;
        card.transform.SetParent(_canvas.transform);
        card.transform.position = position;

        var startPosition = card.transform.localPosition;       
        var moveTime = 0.1f;

        var xPosition = isLeft ? -5f : 5f;
        var yPosition = startPosition.y + 80f;

        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
        {
            card.transform.localPosition = new Vector3(Mathf.Lerp(startPosition.x, xPosition, t), Mathf.Lerp(startPosition.y, yPosition, t), 0f);

            yield return null;
        }

        startPosition = new Vector3(xPosition, yPosition, 0f);
        var shakeTime = 1f;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / shakeTime)
        {
            card.transform.localPosition = startPosition + new Vector3
            (
                UnityEngine.Random.Range(0f, 5f),
                UnityEngine.Random.Range(0f, 5f),
                UnityEngine.Random.Range(0f, 5f)
            );

            yield return null;
        }

        card.transform.localPosition = startPosition;

        if(onComplete != null)
        {
            onComplete();
        }
    }

    private IEnumerator AnimateCombination(HalfCard leftCard, HalfCard rightCard)
    {
        _screenFlash.Flash();
        var combinedCardObject = Instantiate(Resources.Load("Prefabs/CombinedCard")) as GameObject;
        combinedCardObject.transform.SetParent(_canvas.transform);
        combinedCardObject.transform.localPosition = new Vector3(0f, leftCard.transform.localPosition.y, 0f);

        var combinedCard = combinedCardObject.GetComponent<Card>();
        combinedCard.SetValue(leftCard.Value);
        combinedCard.Cost = leftCard.Cost;
        combinedCard.SetSymbol(rightCard.Symbol);
        
        GameObject.Destroy(leftCard.gameObject);
        GameObject.Destroy(rightCard.gameObject);        

        yield return new WaitForSeconds(0.5f);

        combinedCard.GetComponent<CombinedCard>().DoStart();
        combinedCard.GetComponent<CombinedCard>().ApplyEffects();

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(combinedCard.AnimateDiscard(() =>
        {
            GameManager.Instance.Busyness--;  
        }, combinedCard));
    }
}
