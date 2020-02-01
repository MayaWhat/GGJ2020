using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{    
    [SerializeField]
    private List<EnemyCard> _cards;

    public List<EnemyCard> GetCards()
    {
        return _cards;
    }
}
