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

    protected PlayerEnergy _playerEnergy;

    protected DiscardPile _discardPile;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        if (_cardValueObject != null) {
            _cardValueImage = _cardValueObject.GetComponent<Image>();
            _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Numbers/" + _value.ToString());
        }        
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanBePlayed()) {
            // highlight slightly
        }
    }

    public abstract void PlayMe();

    public virtual bool CanBePlayed() 
    {
        return _cost <= _playerEnergy.Energy && GameManager.Instance.Phase == GameplayPhase.PlayCards && !Player.Instance.IsDead && !Enemy.Instance.IsDead;
    }

    public void AttemptToPlay()
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
