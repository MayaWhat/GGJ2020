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

    private SpriteRenderer _spriteRenderer;
    private Player _player;
    private GameObject _enemyHealthUI;

    public int Health 
    {
        get 
        {
            return _hp;
        }
    }

    public int StartingHealth
    {
        get
        {
            return _startingHp;
        }
    }

    private int _block;

    public int Block
    {
        get
        {
            return _block;
        }
        set 
        {
            _block = value;
        }
    }

    public void Appear()
    {
        _spriteRenderer.enabled = true;
        foreach (Transform uiElement in _enemyHealthUI.transform)
        {
            uiElement.gameObject.SetActive(true);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
        _damage = _startingDamage;
        _player = FindObjectOfType<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
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
        
        StartCoroutine(FadeTo(0, 0.1f, true));

        if (_hp <= 0) {
            Die();
        }
    }

    public void Die() {
        Debug.Log("I, the enemy, am dead :(");
    }

    IEnumerator FadeTo(float aValue, float aTime, bool toRed)
    {
        float gb = _spriteRenderer.color.g;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, Mathf.Lerp(gb, aValue, t), Mathf.Lerp(gb, aValue, t), 1);
            _spriteRenderer.color = newColor;
            yield return null;
        }

        if (toRed)
        {
            StartCoroutine(FadeTo(1.0f, 0.1f, false));
        }

        if (!toRed)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

}
