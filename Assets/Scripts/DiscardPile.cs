using System.Collections.Generic;
using UnityEngine;

public class DiscardPile : MonoBehaviour
{    
    [SerializeField]
    private List<Card> _cards;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
        UpdateUIText();
    }

    public void UpdateUIText() {
        _uitext.SetText(CardCount.ToString());
    }
}
