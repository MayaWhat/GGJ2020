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

    public void TakeDamage(int damage)
    {
        _hp -= damage;
        Debug.Log($"Enemy took {damage} damage.");

        if (_hp <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("I, the enemy, am dead :(");
    }
}
