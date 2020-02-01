public class EnemyStrikeCard : EnemyCard
{   
    public override void PlayMe()
    {
        var player = FindObjectOfType<Player>();
        player.TakeDamage(_value);        
        transform.SetParent(_discardPile.transform, false);
    }
}
