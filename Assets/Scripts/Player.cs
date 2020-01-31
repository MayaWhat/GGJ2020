using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private int _hp;

    [SerializeField]
    private int _startingHp;

    [SerializeField]
    private int _currentBlock;
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damageValue)
    {
        var mitigatedDamageValue = Math.Max(0, damageValue - _currentBlock);
        _currentBlock = Math.Max(0, _currentBlock - damageValue);

        if (mitigatedDamageValue > 0)
        {
            _hp -= mitigatedDamageValue;
        }

        Debug.Log($"Player struck with {damageValue} damage, mitigated to {mitigatedDamageValue}. New block {_currentBlock}. New hp {_hp}.");
    }

    public void GainBlock(int blockValue)
    {
        _currentBlock += blockValue;

        Debug.Log($"Player gained {blockValue} block. New block value {_currentBlock}.");
    }
}
