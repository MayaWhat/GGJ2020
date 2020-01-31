using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{    
    [SerializeField]
    private List<Card> _cards;

    [SerializeField]
    private UIText _uitext;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(Card card)
    {
        _cards.Add(card);
        _uitext.SetText(_cards.Count.ToString());
    }
}
