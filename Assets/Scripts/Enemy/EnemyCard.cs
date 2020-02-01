using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyCard : MonoBehaviour
{
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected bool _isAttack;
    [SerializeField]
    protected GameObject _cardValueObject;
    protected Image _cardValueImage;
    protected EnemyDiscardPile _discardPile;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<EnemyDiscardPile>();
        _cardValueImage = _cardValueObject.GetComponent<Image>();
        _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Monster Cards/Numbers/" + _value.ToString() + "_monster");
    }

    public abstract void PlayMe();    
}
