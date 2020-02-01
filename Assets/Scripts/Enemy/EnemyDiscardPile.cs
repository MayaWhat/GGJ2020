using System.Collections.Generic;
using UnityEngine;

public class EnemyDiscardPile : MonoBehaviour
{    
    [SerializeField]
    private List<EnemyCard> _cards;

    public void Add(Transform card)
    {
        card.SetParent(transform, false);
    }
}
