using System;
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
    public bool IsDead { get; private set; }

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

    private EnemyDrawPile _enemyDrawPile;

    [SerializeField]
    private EnemyDeck _startDeck;

    private EnemyHand _enemyHand;
    private static Enemy _instance;

    public static Enemy Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Enemy>();
			}
			return _instance;
		}
	}
    
	public delegate void DeathAction();
	public static event DeathAction OnDeath;

    public void Appear()
    {
        _spriteRenderer.enabled = true;
        foreach (Transform uiElement in _enemyHealthUI.transform)
        {
            uiElement.gameObject.SetActive(true);
        }

        _enemyDrawPile.Init(_startDeck.GetCards());
        _enemyHand.DrawHand();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
        _damage = _startingDamage;
        _player = FindObjectOfType<Player>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyHealthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
        _enemyDrawPile = FindObjectOfType<EnemyDrawPile>();
        _enemyHand = FindObjectOfType<EnemyHand>();
    }

    public void DrawHand()
    {
        _enemyHand.DrawHand();
    }

    public void DoTurn()
    {
        _enemyHand.PlayAllCards();

        Debug.Log("Enemy did turn.");
    }

    public void TakeDamage(int damageValue)
    {
        var mitigatedDamageValue = Math.Max(0, damageValue - _block);
        _block = Math.Max(0, _block - damageValue);

        if (mitigatedDamageValue > 0)
        {
            _hp -= mitigatedDamageValue;
            StartCoroutine(FadeTo(0, 0.1f, true));
        }

        Debug.Log($"Enemy struck with {damageValue} damage, mitigated to {mitigatedDamageValue}. New block {_block}. New hp {_hp}.");
        
        if (_hp <= 0) {
            Die();
        }
    }

    public void GainBlock(int block)
    {
        Block += block;
    }

    public void Die() 
    {
        StartCoroutine(FadeOut(2f));
        IsDead = true;
        OnDeath();
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

    IEnumerator FadeOut(float aTime)
    {
        float newAlphaValue = 0;
        float alpha = _spriteRenderer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _spriteRenderer.color = newColor;
            yield return null;
        }
    }

    public void ClearBlock()
    {
        _block = 0;
    }
}
