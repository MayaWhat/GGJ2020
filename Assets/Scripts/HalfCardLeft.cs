using System;

public class HalfCardLeft : Card
{
    public void SetValue(int value) 
    {
        _value = value;
    }

    public override bool CanBePlayed() {
        return false;
    }

    protected override void DoEffect(Action whenDone)
    {
        // plz don't, we don't know what could happen
    }
}
