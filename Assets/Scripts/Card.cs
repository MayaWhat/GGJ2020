using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField]
    protected int _cost;
    [SerializeField]
    protected int _value;
    [SerializeField]
    protected bool _isAttack;

    protected DiscardPile _discardPile;

    // Start is called before the first frame update
    protected void Start()
    {
        _discardPile = FindObjectOfType<DiscardPile>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayMe();
}
