public class HalfCardLeft : Card
{
    public void SetValue(int value) 
    {
        _value = value;
    }

    public override bool CanBePlayed() {
        return false;
    }

    public override void PlayMe()
    {
        // plz don't, we don't know what could happen
    }
}
