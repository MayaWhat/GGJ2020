using System;

public class StrikeCard : Card
{
    protected override void DoEffect(Action whenDone)
    {
        GameManager.Instance.Enemy.TakeDamage(_value);
        if(whenDone != null)
        {
            whenDone();
        }
    }
}
