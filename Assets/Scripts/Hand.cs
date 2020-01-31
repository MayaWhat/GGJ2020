using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private DrawPile _drawPile;
    [SerializeField]
    private List<Card> _cards;

    // Start is called before the first frame update
    void Start()
    {
        _drawPile = FindObjectOfType<DrawPile>();
        gameObject.GetComponent<Renderer>().material.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawHand()
    {
        _cards = _drawPile.DrawCards(2);

        foreach (var card in _cards)
        {
            var cardObject = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity);
            cardObject.transform.SetParent(transform, false);
        }
    }
}
