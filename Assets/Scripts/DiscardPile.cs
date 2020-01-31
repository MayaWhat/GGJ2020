using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiscardPile : MonoBehaviour
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

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
    }
}
