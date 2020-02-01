using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{
    private GameManager _gameManager;
    private Button _button;
    private Hand _hand;
    private Sprite _youMayEndTurnSprite;
    private Sprite _youCantEndTurnSprite;
    private Sprite _youCanOnlyEndTurnSprite;
    private Image _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _spriteRenderer = GetComponent<Image>();
        _hand = FindObjectOfType<Hand>();
        _button = GetComponent<Button>();
        _youMayEndTurnSprite = Resources.Load<Sprite>("Sprites/End Turn Button/endturn_but_enabled");
        _youCantEndTurnSprite = Resources.Load<Sprite>("Sprites/End Turn Button/endturn_but_disabled");
        _youCanOnlyEndTurnSprite = Resources.Load<Sprite>("Sprites/End Turn Button/endturn_but_enabled_hover");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hand.CanPlaySomething() && !Player.Instance.IsDead && !Enemy.Instance.IsDead) 
        {
            _spriteRenderer.sprite = _youCanOnlyEndTurnSprite;
        }
        else 
        {
            _spriteRenderer.sprite = _youMayEndTurnSprite;
        }
    }

    public void EndPlayerTurn() {
        _gameManager.PlayerEndedTurn();
    }

    private void PlayerDied()
    {
        _button.interactable = false;
    }

    private void EnemyDied()
    {
        _button.interactable = false;
    }

    void OnEnable()
	{
		Player.OnDeath += PlayerDied;
 		GameManager.Instance.Enemy.OnDeath += EnemyDied;
	}

	void OnDisable()
	{
		Player.OnDeath -= PlayerDied;
		GameManager.Instance.Enemy.OnDeath -= EnemyDied;
	}
}
