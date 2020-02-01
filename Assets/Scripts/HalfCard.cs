public abstract class HalfCard : Card 
{
    public bool IsLeftHalf;
    public bool Clickable = true;

    public override void AttemptToPlay() {
        if (!Clickable) {
            return;
        }
        _playerHand.HalfCardSelected(this);
    }
}