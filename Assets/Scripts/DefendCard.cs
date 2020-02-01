public class DefendCard : Card
{

    public override void PlayMe()
    {
        _playerEnergy.Energy -= _cost;
        var player = FindObjectOfType<Player>();
        player.GainBlock(_value);
        Split();
    }
}
