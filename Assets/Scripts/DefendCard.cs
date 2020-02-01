using System;

public class DefendCard : Card
{

    protected override void DoEffect(Action whenDone)
    {
        _player.GainBlock(_value);

        if(whenDone != null)
        {
            whenDone();
        }
    }
}
