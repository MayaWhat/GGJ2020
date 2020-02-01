using System;

public class CombinedCard : Card
{
    protected override void DoEffect(Action whenDone)
    {
        if (_cardSymbol == CardSymbol.Attack) {
            GameManager.Instance.Enemy.TakeDamage(_value);
        } 
        else {
            _player.GainBlock(_value);
        }

        whenDone();
    }
}
