public class StrikeCard : Card
{
    public override void PlayMe()
    {
        _playerEnergy.Energy -= _cost;
        GameManager.Instance.Enemy.TakeDamage(_value);
        Split();
    }
}
