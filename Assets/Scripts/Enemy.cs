using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    [SerializeField]
    private int _hp;

    [SerializeField]
    private int _startingHp;
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
