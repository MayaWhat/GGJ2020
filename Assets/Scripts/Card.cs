using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Card : MonoBehaviour
{
    [SerializeField]
    protected int _cost;
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected int _increaseValue;
    [SerializeField]
    protected CardSymbol _cardSymbol;
    [SerializeField]
    protected GameObject _cardValueObject;
    protected Image _cardValueImage;
    [SerializeField]
    protected GameObject _cardSymbolObject;
    protected Image _cardSymbolImage;
    [SerializeField]
    protected GameObject _cardBackObject;
    [SerializeField]
    private GameObject _cardCostObject;
    private Text _cardCostText;
    protected Image _cardBackImage;
    protected PlayerEnergy _playerEnergy;
    protected DiscardPile _discardPile;
    protected Hand _playerHand;

    protected Player _player;

    protected Canvas _canvas;
    protected ScreenFlash _screenFlash;

    private ParticleSystem _splitParticles;

    private bool _currentlyResolving = false;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        _playerEnergy = FindObjectOfType<PlayerEnergy>();
        _playerHand = FindObjectOfType<Hand>();
        _player = FindObjectOfType<Player>();
        
        var particlesPrefab = Resources.Load<ParticleSystem>("Prefabs/CardSplitParticles");
        _splitParticles = Instantiate(particlesPrefab).GetComponent<ParticleSystem>();
        _splitParticles.transform.SetParent(transform);
        _splitParticles.transform.localPosition = new Vector3(0f, 0f, 0f);

        if (_cardValueObject != null) {
            _cardValueImage = _cardValueObject.GetComponent<Image>();
            SetValue(_value);
        }

        if (_cardSymbolObject != null) {
            _cardSymbolImage = _cardSymbolObject.GetComponent<Image>();
            SetSymbol(_cardSymbol);
        }   

        if (_cardBackObject != null) {
            _cardBackImage = _cardBackObject.GetComponent<Image>();
        }

        if (_cardCostObject != null) {
            _cardCostText = _cardCostObject.GetComponent<Text>();
            _cardCostText.text = _cost.ToString();
        }

        _canvas = FindObjectOfType<Canvas>();     
        _screenFlash = FindObjectOfType<ScreenFlash>();
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
        if (_cardValueImage != null)
        {
            _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Numbers/" + _value.ToString());
        }
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

        if (_cardSymbolImage != null)
        {
            var symbolPath = string.Empty;
            if (_cardSymbol == CardSymbol.Attack) {
                symbolPath = "sword";
            } else {
                symbolPath = "shield";
            }
            _cardSymbolImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Icons/" + symbolPath);
        }
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
            if (_cardCostObject != null) {
                _cardCostText = _cardCostObject.GetComponent<Text>();
                _cardCostText.text = _cost.ToString();
            }
        }
    }
    
    public void PlayMe()
    {
        _currentlyResolving = true;
        GameManager.Instance.Busyness++;
        _playerEnergy.Energy -= _cost;
        StartCoroutine(AnimatePlay());
    }

    private IEnumerator AnimatePlay()
    {
        GameManager.Instance.Sounds.CastSpell.Play();

        var position = transform.position;

        transform.SetParent(_canvas.transform);

        transform.position = position;

        var startPosition = transform.localPosition;       

        var moveTime = 0.1f;

        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
        {
            var movement = 80f * t;
            transform.localPosition = startPosition + new Vector3(0f, movement, 0f);

            yield return null;
        }

        startPosition += new Vector3(0f, 80f, 0f);
        var shakeTime = 1f;
        for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / shakeTime)
        {
            transform.localPosition = startPosition + new Vector3
            (
                UnityEngine.Random.Range(0f, 5f),
                UnityEngine.Random.Range(0f, 5f),
                UnityEngine.Random.Range(0f, 5f)
            );

            yield return null;
        }

        transform.localPosition = startPosition;

        _cardBackImage.enabled = false;
        _cardSymbolImage.enabled = false;
        _cardValueImage.enabled = false;

        var halves = Split();
        _screenFlash.Flash();

        _splitParticles.transform.SetParent(_canvas.transform);
        _splitParticles.transform.position = transform.position;
        _splitParticles.Play();

        // TODO: remove once the card effects are animated
        yield return new WaitForSeconds(1f);

        DoEffect(() =>
        {
            StartCoroutine(AnimateDiscard(() =>
            {
                halves.LeftHalf.Clickable = true;
                halves.RightHalf.Clickable = true;
                GameObject.Destroy(transform.gameObject);
                GameObject.Destroy(_splitParticles);
                GameManager.Instance.Busyness--;  
            }, halves.LeftHalf, halves.RightHalf));                        
        });
    }

    public IEnumerator AnimateDiscard(Action onComplete, params Card[] cards)
    {
        var discardDestination = new Vector3(500f, -500f, 0f);
        for(var i = 0; i < cards.Length; i++)
        {
            GameManager.Instance.Sounds.PlayerDrawCard.Play();
            var startPosition = cards[i].transform.localPosition;

            var moveTime = 0.25f;
            for (var t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
            {
                cards[i].transform.localPosition = new Vector3
                (
                    Mathf.Lerp(startPosition.x, discardDestination.x, t),
                    Mathf.Lerp(startPosition.y, discardDestination.y, t),
                    0f
                );

                yield return null;
            }

            cards[i].transform.SetParent(_discardPile.transform);
        }  

        onComplete();

            
    }

    protected abstract void DoEffect(Action whenDone);

    public virtual bool CanBePlayed() 
    {
        return _cost <= _playerEnergy.Energy 
            && GameManager.Instance.Phase == GameplayPhase.PlayCards 
            && !Player.Instance.IsDead 
            && (!GameManager.Instance.Enemy?.IsDead ?? false)
            && !_currentlyResolving;
    }

    public virtual void AttemptToPlay()
    {
        _playerHand.FullCardSelected();

        if (!CanBePlayed()) {
            Debug.Log("You can't do that!");
            CantPlayFlash();
            return;
        }

        PlayMe();
    }

    public void CantPlayFlash() {
        StartCoroutine(Flash(0, 0.1f, true));
    }

    IEnumerator Flash(float aValue, float aTime, bool toRed)
    {
        float gb = _cardBackImage.color.g;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, Mathf.Lerp(gb, aValue, t), Mathf.Lerp(gb, aValue, t), 1);
            _cardBackImage.color = newColor;
            yield return null;
        }

        if (toRed)
        {
            StartCoroutine(Flash(1.0f, 0.1f, false));
        }

        if (!toRed)
        {
            _cardBackImage.color = new Color(1, 1, 1, 1);
        }
    }

    public (HalfCardLeft LeftHalf, HalfCardRight RightHalf) Split() {
        var leftHalfObject = Instantiate(Resources.Load("Prefabs/HalfCardLeft")) as GameObject;
        var rightHalfObject = Instantiate(Resources.Load("Prefabs/HalfCardRight")) as GameObject;

        var leftHalf = leftHalfObject.GetComponent<HalfCardLeft>();
        var rightHalf = rightHalfObject.GetComponent<HalfCardRight>();

        leftHalf.SetValue(_value);
        leftHalf.Cost = _cost;
        leftHalf.IsLeftHalf = true;
        rightHalf.SetSymbol(_cardSymbol);
        leftHalf.Clickable = false;
        rightHalf.Clickable = false;

        leftHalf.transform.SetParent(_canvas.transform);
        leftHalf.transform.position = transform.position + new Vector3(-10f, 0f, 0f);
        rightHalf.transform.SetParent(_canvas.transform);
        rightHalf.transform.position = transform.position + new Vector3(10f, 0f, 0f);

        return (leftHalf, rightHalf);
    }

    public enum CardSymbol
    {
        Attack = 0,
        Block = 1
    }
}
