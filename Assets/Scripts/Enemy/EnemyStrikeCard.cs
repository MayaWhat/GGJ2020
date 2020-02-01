public class EnemyStrikeCard : EnemyCard
{   
    public override void PlayMe()
    {
        var player = FindObjectOfType<Player>();
        player.TakeDamage(_value);        
        _value += _increaseValue;
        _cardValueText.text = _value.ToString();
        transform.SetParent(_discardPile.transform, false);
    }
}
