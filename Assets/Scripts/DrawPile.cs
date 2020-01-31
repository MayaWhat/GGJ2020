using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawPile : MonoBehaviour
{    
    [SerializeField]
    private List<Card> _cards;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(List<Card> cards)
    {
        _cards = cards;
    }

    public List<Card> DrawCards(int drawAmount)
    {
        return _cards.Take(drawAmount).ToList();
    }
}
