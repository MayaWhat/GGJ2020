using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeCard : Card
{    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayMe() 
    {
        var enemy = FindObjectOfType<Enemy>();
        enemy.TakeDamage(_value);
        var discardPile = FindObjectOfType<DiscardPile>();
        transform.SetParent(discardPile.transform, false);
    }
}
