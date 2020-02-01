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
    protected Text _cardValueText;
    protected EnemyDiscardPile _discardPile;
    [SerializeField]
    protected int _increaseValue;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<EnemyDiscardPile>();
        _cardValueText = _cardValueObject.GetComponent<Text>();
        _cardValueText.text = _value.ToString();
    }

    public abstract void PlayMe();    
}
