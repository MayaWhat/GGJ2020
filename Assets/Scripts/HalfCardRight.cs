using System;
public class HalfCardRight : HalfCard
{
    public override bool CanBePlayed() {
        return false;
    }

    protected override void DoEffect(Action whenDone)
    {
        // plz don't, we don't know what could happen
    }
}
