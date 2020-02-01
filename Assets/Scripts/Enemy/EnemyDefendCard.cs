public class EnemyDefendCard : EnemyCard
{
    public override void PlayMe()
    {
        var enemy = FindObjectOfType<Enemy>();
        enemy.GainBlock(_value);
        transform.SetParent(_discardPile.transform, false);
    }
}
