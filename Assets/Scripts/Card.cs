using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [SerializeField]
    protected int _cost;
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected CardSymbol _cardSymbol;
    [SerializeField]
    protected GameObject _cardValueObject;
    protected Image _cardValueImage;
    [SerializeField]
    protected GameObject _cardSymbolObject;
    protected Image _cardSymbolImage;
    protected PlayerEnergy _playerEnergy;
    protected DiscardPile _discardPile;
    protected Hand _playerHand;

    protected Player _player;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _playerHand = FindObjectOfType<Hand>();
        _player = FindObjectOfType<Player>();

        if (_cardValueObject != null) {
            _cardValueImage = _cardValueObject.GetComponent<Image>();
            _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Numbers/" + _value.ToString());
        }

        if (_cardSymbolObject != null) {
            _cardSymbolImage = _cardSymbolObject.GetComponent<Image>();
            var symbolPath = string.Empty;
            if (_cardSymbol == CardSymbol.Attack) {
                symbolPath = "sword";
            } else {
                symbolPath = "shield";
            }
            var sprite = Resources.Load<Sprite>("Sprites/Cards/Icons/" + symbolPath);
            _cardSymbolImage.sprite = sprite;
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanBePlayed()) {
            // highlight slightly
        }
    }

    public void SetValue(int value) 
    {
        _value = value;
    }

    public int Value
    {
        get
        {
            return _value;
        }
    }

    public CardSymbol Symbol
    {
        get
        {
            return _cardSymbol;
        }
    }

    public void SetSymbol (CardSymbol symbol)
    {
        _cardSymbol = symbol;
    }

    public int Cost
    {
        get
        {
            return _cost;
        }
        set
        {
            _cost = value;
        }
    }

    public abstract void PlayMe();

    public virtual bool CanBePlayed() 
    {
        return _cost <= _playerEnergy.Energy && GameManager.Instance.Phase == GameplayPhase.PlayCards && !Player.Instance.IsDead && !Enemy.Instance.IsDead;
    }

    public virtual void AttemptToPlay()
    {
        if (!CanBePlayed()) {
            Debug.Log("You can't do that!");
            return;
        }

        PlayMe();
    }

    public void Split() {
        var leftHalfObject = Instantiate(Resources.Load("Prefabs/HalfCardLeft")) as GameObject;
        var rightHalfObject = Instantiate(Resources.Load("Prefabs/HalfCardRight")) as GameObject;

        var leftHalf = leftHalfObject.GetComponent<HalfCardLeft>();
        var rightHalf = rightHalfObject.GetComponent<HalfCardRight>();

        leftHalf.SetValue(_value);
        leftHalf.IsLeftHalf = true;
        rightHalf.SetSymbol(_cardSymbol);

        leftHalf.transform.SetParent(_discardPile.transform);
        rightHalf.transform.SetParent(_discardPile.transform);
        GameObject.Destroy(transform.gameObject);
    }

    public enum CardSymbol 
    {
        Attack = 0,
        Block = 1
    }
}
