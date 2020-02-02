using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{    
    [SerializeField]
    private int _startingHp;

    [SerializeField]
    private int _hp;

    [SerializeField]
    private int _currentBlock;

    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private GameObject _hitMarker;
    private SpriteRenderer _hitMarkerRenderer;
    [SerializeField]
    private GameObject _shieldMarker;
    private SpriteRenderer _shieldMarkerRenderer;
    [SerializeField]
    private GameObject _hitNumber;
    private TextMesh _hitNumberRenderer;
    protected Speechbubble _playerSpeechBubble;
    protected Image _bubbleImage;
    public int DamageTaken { get; set; }

    public bool IsDead { get; private set; }

    public int Health {
        get {
            return _hp;
        }
    }

    private static Player _instance;

	public static Player Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<Player>();
			}
			return _instance;
		}
	}
    
	public delegate void DeathAction();
	public static event DeathAction OnDeath;
	public delegate void BlockAction();
	public static event BlockAction OnBlock;

    public int StartingHealth {
        get {
            return _startingHp;
        }
    }

    public int Block
    {
        get
        {
            return _currentBlock;
        }
        set 
        {
            _currentBlock = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _hp = _startingHp;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _hitMarkerRenderer = _hitMarker.GetComponent<SpriteRenderer>();
        _shieldMarkerRenderer = _shieldMarker.GetComponent<SpriteRenderer>();
        _hitNumberRenderer = _hitNumber.GetComponent<TextMesh>();
        _playerSpeechBubble = FindObjectOfType<Speechbubble>();
        _bubbleImage = _playerSpeechBubble.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hp < 0 && !IsDead)
        {
            JustDieAlready();
        }
    }

    private void JustDieAlready()
    {
        IsDead = true;

        StartCoroutine(FadeOut(2f));

        OnDeath();
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

    public void StartTurn()
    {
        _currentBlock = 0;
        Debug.Log("Set players block to 0 at start of turn.");
    }

    public void TakeDamage(int damageValue)
    {

        var didBlock = false;
        GameManager.Instance.Sounds.WitchHurt.Play();
        GameManager.Instance.Sounds.CombatImpact.Play();

        if(_hp <= 0)
        {
            return;
        }

        var oldBlock = _currentBlock;
        var mitigatedDamageValue = Math.Max(0, damageValue - _currentBlock);
        _currentBlock = Math.Max(0, _currentBlock - damageValue);

        if (oldBlock != _currentBlock)
        {
            didBlock = true;
            OnBlock();
        }

        if (mitigatedDamageValue > 0)
        {
            _hp -= mitigatedDamageValue;
            StartCoroutine(FadeRed(0, 0.1f, true));
            if (mitigatedDamageValue >= 5) {
                BigOof();
            }
        }
        else
        {
            _hitNumberRenderer.text = "0";
            StartCoroutine(FadeHitNumber(1f, .1f, () => Invoke("FadeHitNumberOut", 3f)));
        }
        StartCoroutine(FadeHitMarker(1f, .1f, () => Invoke("FadeHitMarkerOut", 1f)));
        StartCoroutine(FadeHitNumber(1f, .1f, () => Invoke("FadeHitNumberOut", 2f)));
        if (didBlock)
        {
            StartCoroutine(FadeShieldMarker(1f, .1f, () => Invoke("FadeShieldMarkerOut", 1f)));
        }
        UpdateDamageTaken(mitigatedDamageValue);
        Debug.Log($"Player struck with {damageValue} damage, mitigated to {mitigatedDamageValue}. New block {_currentBlock}. New hp {_hp}.");
    }

    public void UpdateDamageTaken(int damage)
    {
        _hitNumberRenderer.text = damage.ToString();
    }

    
    private void FadeHitNumberOut()
    {
        StartCoroutine(FadeHitNumber(0, .4f, null));
    }

    IEnumerator FadeHitNumber(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _hitNumberRenderer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 0, 0, Mathf.Lerp(alpha, newAlphaValue, t));
            _hitNumberRenderer.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    private void FadeHitMarkerOut()
    {
        StartCoroutine(FadeHitMarker(0, .4f, null));
    }
    private void FadeShieldMarkerOut()
    {
        StartCoroutine(FadeShieldMarker(0, .4f, null));
    }

    public void GainBlock(int blockValue)
    {
        _currentBlock += blockValue;
        GameManager.Instance.Sounds.CombatBlock.Play();

        Debug.Log($"Player gained {blockValue} block. New block value {_currentBlock}.");
    }

    IEnumerator FadeShieldMarker(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _shieldMarkerRenderer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _shieldMarkerRenderer.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    IEnumerator FadeHitMarker(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _hitMarkerRenderer.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _hitMarkerRenderer.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    IEnumerator FadeRed(float aValue, float aTime, bool toRed)
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
            StartCoroutine(FadeRed(1.0f, 0.1f, false));
        }

        if (!toRed)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    public void BigOof()
    {
        _playerSpeechBubble.enabled = true;
        _bubbleImage.sprite = Resources.Load<Sprite>("Sprites/Speech Bubble/bubble_hurt_" + Random.Range(1,3));
        StartCoroutine(FadeBubble(1f, .1f, () => Invoke("FadeBubbleOut", 1f)));
    }

    private void FadeBubbleOut()
    {
        StartCoroutine(FadeBubble(0, .4f, () => Invoke("HideBubble", 1f)));
    }

    IEnumerator FadeBubble(float newAlphaValue, float aTime, Action onFinish)
    {
        float alpha = _bubbleImage.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, newAlphaValue, t));
            _bubbleImage.color = newColor;
            yield return null;
        }

        if (onFinish != null)
        {
            onFinish();
        }
    }

    public void HideBubble() {
       _bubbleImage.color = (new Color(1,1,1,0));
    }

}
