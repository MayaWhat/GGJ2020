public class DefendCard : Card
{

    public override void PlayMe()
    {
        _playerEnergy.Energy -= _cost;
        _player.GainBlock(_value);
        Split();
    }
}
