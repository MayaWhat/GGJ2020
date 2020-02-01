public abstract class HalfCard : Card 
{
    public bool IsLeftHalf;
    public bool Clickable = true;

    public bool Affordable
    {
        get
        {
            return _playerEnergy.Energy >= _cost;
        }
    }

    public override void AttemptToPlay() {
        if (!Clickable) {
            return;
        }

        if (!Affordable) {
            CantPlayFlash();
            return;
        }
        _playerHand.HalfCardSelected(this);
    }
}