using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject _hitMarker;
    private Image _hitMarkerImage;

    [SerializeField]
    private FMODUnity.StudioEventEmitter _damagedSound;

    [SerializeField]
    private FMODUnity.StudioEventEmitter _actingSound;

    private Image _image;
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
	public event DeathAction OnDeath;

    public delegate void AppearAction();
    public event AppearAction OnAppear;

    public bool ShouldAppear
    {
        get;
        set;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
        _damage = _startingDamage;
        _player = FindObjectOfType<Player>();
        _image = GetComponentInChildren<Image>();
        _enemyHealthUI = GameObject.FindGameObjectWithTag("EnemyHealthUI");
        _enemyDrawPile = FindObjectOfType<EnemyDrawPile>();
        _enemyHand = FindObjectOfType<EnemyHand>();
        _hitMarkerImage = _hitMarker.GetComponent<Image>();
    }

    void Update()
    {
        if (ShouldAppear && !_image.enabled)
        {
            Appear();
        }
    }

    private void Appear()
    {
        _image.enabled = true;
        _image.color = new Color(1, 1, 1, 0f);
        foreach (Transform uiElement in _enemyHealthUI.transform)
        {
            uiElement.gameObject.SetActive(true);
        }

        StartCoroutine(FadeIn(0.75f, () => {
            _enemyDrawPile.Init(_startDeck.GetCards());
            DrawHand(() => OnAppear());
        }));
    }

    public void DrawHand(Action onFinish)
    {
        _enemyHand.DrawHand(onFinish);
    }

    public void DoTurn()
    {
        _actingSound.Play();
        _enemyHand.PlayAllCards();

        Debug.Log("Enemy did turn.");
    }

    public void TakeDamage(int damageValue)
    {
        _damagedSound.Play();
        GameManager.Instance.Sounds.CombatImpact.Play();
        var mitigatedDamageValue = Math.Max(0, damageValue - _block);
        _block = Math.Max(0, _block - damageValue);

        if (mitigatedDamageValue > 0)
        {
            _hp -= mitigatedDamageValue;
            StartCoroutine(FadeTo(0, 0.1f, true));
            StartCoroutine(FadeHitMarker(1f, .1f, () => Invoke("FadeHitMarkerOut", 1f)));
        }

        Debug.Log($"Enemy struck with {damageValue} damage, mitigated to {mitigatedDamageValue}. New block {_block}. New hp {_hp}.");
        
        if (_hp <= 0) {
            _hp = 0;
            Die();
        }
    }

    private void FadeHitMarkerOut()
    {
        StartCoroutine(FadeHitMarker(0, .4f, null));
    }

    IEnumerator FadeHitMarker(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _hitMarkerImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _hitMarkerImage.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    public void GainBlock(int block)
    {
        Block += block;
    }

    public void Die() 
    {
        GameManager.Instance.Busyness++;
        StartCoroutine(FadeOut(2f, () => 
        {
            GameManager.Instance.Busyness--;
            Destroy(this);
        }));
        IsDead = true;
        _enemyHand.DiscardHand();
        OnDeath();
        Debug.Log("I, the enemy, am dead :(");
    }

    IEnumerator FadeIn(float aTime, Action onFinish)
    {
        float newAlphaValue = 1f;
        float alpha = _image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _image.color = newColor;
            yield return null;
        }

        onFinish();
    }

    IEnumerator FadeTo(float aValue, float aTime, bool toRed)
    {
        float gb = _image.color.g;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, Mathf.Lerp(gb, aValue, t), Mathf.Lerp(gb, aValue, t), 1);
            _image.color = newColor;
            yield return null;
        }

        if (toRed)
        {
            StartCoroutine(FadeTo(1.0f, 0.1f, false));
        }

        if (!toRed)
        {
            _image.color = new Color(1, 1, 1, 1);
        }
    }

    IEnumerator FadeOut(float aTime, Action onFinish)
    {
        float newAlphaValue = 0;
        float alpha = _image.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _image.color = newColor;
            yield return null;
        }

        onFinish();
    }

    public void ClearBlock()
    {
        _block = 0;

    }
}
