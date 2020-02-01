using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendCard : Card
{
    public override void PlayMe()
    {
        var player = FindObjectOfType<Player>();
        player.GainBlock(_value);
        var discardPile = FindObjectOfType<DiscardPile>();
        transform.SetParent(discardPile.transform, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
