public class HalfCardLeft : HalfCard
{
    public override bool CanBePlayed() {
        return false;
    }

    public override void PlayMe()
    {
        // plz don't, we don't know what could happen
    }

    public override void AttemptToPlay() {
        _playerHand.HalfCardSelected(this);
    }
}
