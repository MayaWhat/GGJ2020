using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [SerializeField]
    protected int _cost;
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected bool _isAttack;
    [SerializeField]
    protected GameObject _cardValueObject;
    protected Image _cardValueImage;

    protected PlayerEnergy _playerEnergy;

    protected DiscardPile _discardPile;


    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        _cardValueImage = _cardValueObject.GetComponent<Image>();
        _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Numbers/" + _value.ToString());
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayMe();

    public bool CanBePlayed() 
    {
        return _cost <= _playerEnergy.Energy;
    }

    public void AttemptToPlay()
    {
        if (!CanBePlayed()) {
            Debug.Log("You can't do that!");
            return;
        }

        PlayMe();
    }
}
