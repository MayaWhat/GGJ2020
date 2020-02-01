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
    protected bool _isAttack;
    [SerializeField]
    protected GameObject _cardValueObject;
    protected Image _cardValueImage;

    protected DiscardPile _discardPile;


    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
        _cardValueImage = _cardValueObject.GetComponent<Image>();
        _cardValueImage.sprite = Resources.Load<Sprite>("Sprites/Cards/Numbers/" + _value.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayMe();
}
