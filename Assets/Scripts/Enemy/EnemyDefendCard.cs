public class EnemyDefendCard : EnemyCard
{
    public override void PlayMe()
    {
        GameManager.Instance.Enemy.GainBlock(_value);
        transform.SetParent(_discardPile.transform, false);
    }
}
