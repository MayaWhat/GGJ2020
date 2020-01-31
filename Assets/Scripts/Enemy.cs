using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{    
    [SerializeField]
    private int _hp;

    [SerializeField]
    private int _startingHp;

    [SerializeField]
    private int _damage;
    [SerializeField]
    private int _startingDamage;

    private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
        _damage = _startingDamage;
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoTurn()
    {
        _player.TakeDamage(_damage);
        _damage += 2;

        Debug.Log("Enemy did turn.");
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
