public class HalfCardRight : Card
{
    public void SetSymbol (CardSymbol symbol)
    {
        _cardSymbol = symbol;
    }

    public override bool CanBePlayed() {
        return false;
    }

    public override void PlayMe()
    {
        // plz don't, we don't know what could happen
    }
}
