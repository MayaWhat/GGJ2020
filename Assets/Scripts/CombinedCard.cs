using System;

public class CombinedCard : Card
{
    protected override void DoEffect(Action whenDone)
    {
        ApplyEffects();
        Split();
    }

    public void ApplyEffects() {
        if (_cardSymbol == CardSymbol.Attack) {
            GameManager.Instance.Enemy.TakeDamage(_value);
        } 
        else {
            _playerEnergy.Energy -= _cost;
            _player.GainBlock(_value);
        }
    }

    public void DoStart() {
        base.Start();
    }
}
