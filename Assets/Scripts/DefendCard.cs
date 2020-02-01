using System;

public class DefendCard : Card
{

    protected override void DoEffect(Action whenDone)
    {
        var player = FindObjectOfType<Player>();
        player.GainBlock(_value);

        if(whenDone != null)
        {
            whenDone();
        }
    }
}
