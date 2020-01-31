using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeCard : Card
{    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayMe()
    {
        var enemy = FindObjectOfType<Enemy>();
        enemy.TakeDamage(_value);        
        transform.SetParent(_discardPile.transform, false);
        _discardPile.UpdateUIText();
    }
}
