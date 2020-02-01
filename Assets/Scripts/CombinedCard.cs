public class CombinedCard : Card
{
    public override void PlayMe()
    {
        if (_cardSymbol == CardSymbol.Attack) {
            GameManager.Instance.Enemy.TakeDamage(_value);
        } 
        else {
            _playerEnergy.Energy -= _cost;
            _player.GainBlock(_value);
        }
        Split();
    }
}
